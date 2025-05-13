using Microsoft.AspNetCore.Mvc;
using ElegantWebFormApp;
using ElegantWebFormApp.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace ElegantWebFormApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User model)
        {
            // Logging pentru a verifica datele primite
            _logger.LogInformation($"Form submitted - FullName: {model?.FullName ?? "null"}, " +
                                  $"Email: {model?.Email ?? "null"}, " +
                                  $"Phone: {model?.PhoneNumber ?? "null"}");

            // Verificarea stării modelului
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning($"- {state.Key}: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }

            // Verificarea email-ului duplicat
            var emailExists = _context.Useri.Any(u => u.Email == model.Email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Acest email este deja înregistrat.");
                return View(model);
            }

            // Verificarea numărului de telefon duplicat
            var phoneExists = _context.Useri.Any(u => u.PhoneNumber == model.PhoneNumber);
            if (phoneExists)
            {
                ModelState.AddModelError("PhoneNumber", "Acest număr de telefon este deja înregistrat.");
                return View(model);
            }

            try
            {
                _context.Useri.Add(model);
                await _context.SaveChangesAsync();
                ViewBag.Success = "Datele au fost salvate cu succes!";
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Eroare la salvarea datelor utilizatorului");
                ModelState.AddModelError("", "A apărut o eroare la salvarea datelor. Vă rugăm încercați din nou.");
            }

            return View();
        }
    }
}