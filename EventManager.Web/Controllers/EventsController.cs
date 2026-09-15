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

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.Include(e => e.Organizer).ToListAsync();
            return View(events);
        }

        // GET: Events/Details/5
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

        // GET: Events/Create
        [Authorize(Roles = "Organizador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizador")]
        public async Task<IActionResult> Create([Bind("Title,Description,DateTime,WorkloadHours,TotalSlots")] Event @event)
        {
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
            
            // Check limits and duplications
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
    }
}
