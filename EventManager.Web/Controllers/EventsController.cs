using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventManager.Web.Data;
using EventManager.Web.Models;
using System.Security.Claims;

namespace EventManager.Web.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Eventos (Lista)
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.Include(e => e.Organizer).ToListAsync();
            return View(events);
        }

        // GET: Eventos (Lista)/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var @event = await _context.Events
                .Include(e => e.Organizer)
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (@event == null)
                return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.IsRegistered = @event.Registrations.Any(r => r.ParticipantId == currentUserId);

            return View(@event);
        }

        // GET: Eventos (Lista)/Create
        [Authorize(Roles = "Organizador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Criar Evento
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> Create([Bind("Title,Description,DateTime,WorkloadHours,TotalSlots")] Event @event)
        {
            ModelState.Remove("OrganizerId");
            ModelState.Remove("Organizer");
            ModelState.Remove("Registrations");

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                @event.OrganizerId = userId!;
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int id)
        {
            var @event = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (@event.Registrations.Any(r => r.ParticipantId == userId))
            {
                TempData["ErrorMessage"] = "Você já está inscrito neste evento.";
                return RedirectToAction(nameof(Details), new { id = @event.Id });
            }

            if (@event.Registrations.Count >= @event.TotalSlots)
            {
                TempData["ErrorMessage"] = "Limite de vagas atingido para este evento.";
                return RedirectToAction(nameof(Details), new { id = @event.Id });
            }

            var registration = new EventRegistration
            {
                EventId = @event.Id,
                ParticipantId = userId!,
                RegistrationDate = DateTime.Now,
                IsPresenceConfirmed = false
            };

            _context.EventRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Inscrição realizada com sucesso!";
            return RedirectToAction(nameof(Details), new { id = @event.Id });
        }

        // GET: Eventos (Lista)/MyEvents
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> MyEvents()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var events = await _context.Events
                .Include(e => e.Registrations)
                .Where(e => e.OrganizerId == userId)
                .OrderByDescending(e => e.DateTime)
                .ToListAsync();
            return View(events);
        }

        // GET: Eventos (Lista)/Edit/5
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (@event.OrganizerId != userId) return Forbid();

            return View(@event);
        }

        // POST: Editar Evento
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DateTime,WorkloadHours,TotalSlots")] Event eventInput)
        {
            if (id != eventInput.Id) return NotFound();

            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (@event.OrganizerId != userId) return Forbid();

            ModelState.Remove("OrganizerId");
            ModelState.Remove("Organizer");
            ModelState.Remove("Registrations");

            if (ModelState.IsValid)
            {
                @event.Title = eventInput.Title;
                @event.Description = eventInput.Description;
                @event.DateTime = eventInput.DateTime;
                @event.WorkloadHours = eventInput.WorkloadHours;
                @event.TotalSlots = eventInput.TotalSlots;

                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.Id == @event.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(MyEvents));
            }
            return View(eventInput);
        }

        // POST: Excluir Evento
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (@event.OrganizerId != userId) return Forbid();

                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(MyEvents));
        }

        // GET: Eventos/ManageParticipants/5
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> ManageParticipants(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.Registrations)
                .ThenInclude(r => r.Participant)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (@event.OrganizerId != userId) return Forbid();

            return View(@event);
        }

        // POST: Eventos/UpdateParticipant
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> UpdateParticipant(int eventId, int registrationId)
        {
            var @event = await _context.Events.FindAsync(eventId);
            if (@event == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (@event.OrganizerId != userId) return Forbid();

            var registration = await _context.EventRegistrations.FindAsync(registrationId);
            if (registration == null || registration.EventId != eventId) return NotFound();

            var presenceValue = Request.Form["isPresenceConfirmed"].ToString(); registration.IsPresenceConfirmed = presenceValue.Contains("true"); var ratingStr = Request.Form["rating"].ToString(); if(int.TryParse(ratingStr, out int r)) registration.Rating = r; else registration.Rating = null;
            

            _context.Update(registration);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"DEBUG: eventId={eventId} regId={registrationId} formPresence={Request.Form["isPresenceConfirmed"]} boolValue={presenceValue.Contains("true")} formRating={Request.Form["rating"]}";
            return RedirectToAction(nameof(ManageParticipants), new { id = eventId });
        }

        // GET: Eventos/DownloadCertificate/5
        public async Task<IActionResult> DownloadCertificate(int id, [FromServices] EventManager.Web.Services.CertificateService certificateService)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var registration = await _context.EventRegistrations
                .Include(r => r.Event)
                .Include(r => r.Participant)
                .FirstOrDefaultAsync(r => r.Id == id && r.ParticipantId == userId);

            if (registration == null) return NotFound();
            
            if (!registration.IsPresenceConfirmed)
            {
                TempData["ErrorMessage"] = "Seu certificado ainda não está liberado. É necessário confirmação de presença.";
                return RedirectToAction(nameof(MyRegistrations));
            }

            try {
                var pdfBytes = certificateService.GenerateCertificate(registration);
                return File(pdfBytes, "application/pdf", $"Certificado_{registration.Event?.Title}.pdf");
            } catch (System.Exception ex) {
                TempData["ErrorMessage"] = "Erro gerando PDF: " + ex.Message + (ex.InnerException != null ? " | " + ex.InnerException.Message : "");
                return RedirectToAction(nameof(MyRegistrations));
            }
        }

        // GET: Eventos (Lista)/MyRegistrations
        public async Task<IActionResult> MyRegistrations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var registrations = await _context.EventRegistrations
                .Include(r => r.Event)
                .ThenInclude(e => e.Organizer)
                .Where(r => r.ParticipantId == userId)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
                
            return View(registrations);
        }
    }
}
