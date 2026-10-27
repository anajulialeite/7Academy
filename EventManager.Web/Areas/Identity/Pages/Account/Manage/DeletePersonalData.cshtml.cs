using System.ComponentModel.DataAnnotations;
using EventManager.Web.Data;
using EventManager.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Web.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Exclusão definitiva da conta do usuário (direito de eliminação de dados - LGPD, art. 18).
    /// Exige a confirmação da senha e remove também inscrições e tentativas de prova.
    /// </summary>
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeletePersonalDataModel> _logger;

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            ILogger<DeletePersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required(ErrorMessage = "Informe sua senha para confirmar.")]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; } = string.Empty;
        }

        public bool RequirePassword { get; set; }

        /// <summary>Organizadores com eventos cadastrados não podem excluir a conta (os eventos ficariam sem responsável).</summary>
        public bool OwnsEvents { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Usuário não encontrado.");

            RequirePassword = await _userManager.HasPasswordAsync(user);
            OwnsEvents = await _context.Events.AnyAsync(e => e.OrganizerId == user.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound("Usuário não encontrado.");

            RequirePassword = await _userManager.HasPasswordAsync(user);
            OwnsEvents = await _context.Events.AnyAsync(e => e.OrganizerId == user.Id);

            if (OwnsEvents)
            {
                ModelState.AddModelError(string.Empty, "Você é organizador(a) de eventos. Exclua ou transfira seus eventos antes de excluir a conta.");
                return Page();
            }

            if (RequirePassword && !await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Senha incorreta.");
                return Page();
            }

            // Remove os dados vinculados ao aluno: tentativas de prova e inscrições
            var registrationIds = await _context.EventRegistrations
                .Where(r => r.ParticipantId == user.Id)
                .Select(r => r.Id)
                .ToListAsync();

            _context.QuizAttempts.RemoveRange(
                _context.QuizAttempts.Where(a => registrationIds.Contains(a.EventRegistrationId)));
            _context.EventRegistrations.RemoveRange(
                _context.EventRegistrations.Where(r => r.ParticipantId == user.Id));
            await _context.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível excluir a conta. Tente novamente.");
                return Page();
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("Usuário {UserId} excluiu a própria conta.", user.Id);

            TempData["AccountDeleted"] = true;
            return Redirect("~/");
        }
    }
}
