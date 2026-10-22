using EventManager.Web.Data;
using EventManager.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EventManager.Web.Controllers
{
    /// <summary>
    /// Prova de certificação: 10 questões embaralhadas, nota automática,
    /// aprovação com nota >= 7 e espera de 30 minutos após reprovação.
    /// </summary>
    [Authorize]
    public class QuizController : Controller
    {
        public const int PassingScore = 7;
        public static readonly TimeSpan Cooldown = TimeSpan.FromMinutes(30);

        private readonly ApplicationDbContext _context;

        public QuizController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<EventRegistration?> GetOwnRegistrationAsync(int registrationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.EventRegistrations
                .Include(r => r.Event).ThenInclude(e => e!.Questions)
                .FirstOrDefaultAsync(r => r.Id == registrationId && r.ParticipantId == userId);
        }

        // GET: Quiz/Material/5  (5 = id da inscricao) - PDF de estudo do curso
        public async Task<IActionResult> Material(int id, [FromServices] EventManager.Web.Services.StudyMaterialService materialService)
        {
            var registration = await GetOwnRegistrationAsync(id);
            if (registration?.Event == null) return NotFound();

            var pdf = materialService.Generate(registration.Event);
            if (pdf == null)
            {
                TempData["ErrorMessage"] = "Este curso ainda n\u00e3o possui material de estudo.";
                return RedirectToAction("MyRegistrations", "Events");
            }

            return File(pdf, "application/pdf");
        }

        // GET: Quiz/Take/5  (5 = id da inscrição)
        public async Task<IActionResult> Take(int id)
        {
            var registration = await GetOwnRegistrationAsync(id);
            if (registration == null) return NotFound();

            // Quem já foi aprovado pode refazer a prova quantas vezes quiser, sem tempo de espera
            bool alreadyApproved = registration.Rating >= PassingScore;

            var questions = registration.Event!.Questions.ToList();
            if (questions.Count == 0)
            {
                TempData["ErrorMessage"] = "Este evento ainda não possui prova cadastrada.";
                return RedirectToAction("MyRegistrations", "Events");
            }

            // Tempo de espera de 30 minutos após reprovação
            var lastAttempt = await _context.QuizAttempts
                .Where(a => a.EventRegistrationId == id)
                .OrderByDescending(a => a.AttemptDate)
                .FirstOrDefaultAsync();

            if (!alreadyApproved && lastAttempt != null && !lastAttempt.Passed)
            {
                var unlockAt = lastAttempt.AttemptDate.Add(Cooldown);
                if (DateTime.Now < unlockAt)
                {
                    ViewBag.UnlockAt = unlockAt;
                    ViewBag.LastScore = lastAttempt.Score;
                    return View("Cooldown", registration);
                }
            }

            // Mesmas questões, ordem embaralhada a cada tentativa
            ViewBag.Questions = questions.OrderBy(_ => Random.Shared.Next()).ToList();
            return View(registration);
        }

        // POST: Quiz/Submit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int id, Dictionary<int, string> answers)
        {
            var registration = await GetOwnRegistrationAsync(id);
            if (registration == null) return NotFound();

            // Proteção extra: não permite burlar o tempo de espera reenviando o formulário
            var lastAttempt = await _context.QuizAttempts
                .Where(a => a.EventRegistrationId == id)
                .OrderByDescending(a => a.AttemptDate)
                .FirstOrDefaultAsync();
            bool wasApproved = registration.Rating >= PassingScore;
            if (!wasApproved && lastAttempt != null && !lastAttempt.Passed && DateTime.Now < lastAttempt.AttemptDate.Add(Cooldown))
            {
                return RedirectToAction(nameof(Take), new { id });
            }

            var questions = registration.Event!.Questions.ToList();
            int correct = questions.Count(q => answers.TryGetValue(q.Id, out var a) && a == q.CorrectOption);

            // Nota de 0 a 10, proporcional aos acertos
            int score = questions.Count == 0 ? 0 : (int)Math.Round(correct * 10.0 / questions.Count);
            bool passed = score >= PassingScore;

            _context.QuizAttempts.Add(new QuizAttempt
            {
                EventRegistrationId = id,
                Score = score,
                Passed = passed,
                AttemptDate = DateTime.Now
            });

            registration.Rating = Math.Max(score, registration.Rating ?? 0);
            if (passed) registration.IsPresenceConfirmed = true; // libera o certificado

            await _context.SaveChangesAsync();

            ViewBag.Score = score;
            ViewBag.Correct = correct;
            ViewBag.Total = questions.Count;
            ViewBag.Passed = passed;
            ViewBag.AlreadyApproved = wasApproved;
            ViewBag.UnlockAt = DateTime.Now.Add(Cooldown);
            return View("Result", registration);
        }
    }
}
