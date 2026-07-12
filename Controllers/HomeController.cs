using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DrKchida.Models;
using DrKchida.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DrKchida.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> _logger, ApplicationDbContext context)
        {
            this._logger = _logger;
            this._context = context;
        }

        public IActionResult Index() => View();
        public IActionResult About() => View();
        [Route("soins-esthetiques")]
        public async Task<IActionResult> Services()
        {
            var categories = await _context.ServiceCategories.Include(c => c.Treatments).ToListAsync();
            return View(categories);
        }

        [Route("collection/{service}")]
        public async Task<IActionResult> ServiceDetails(string service)
        {
            if (string.IsNullOrEmpty(service)) return RedirectToAction("Services");

            var currentService = await _context.ServiceCategories
                .Include(c => c.Treatments)
                .FirstOrDefaultAsync(c => c.Slug == service.ToLower());
            
            if (currentService == null) return NotFound();

            return View(currentService);
        }

        [Route("soin/{id}/{slug?}")]
        public async Task<IActionResult> TreatmentDetails(int id, string slug = null)
        {
            var treatment = await _context.Treatments
                .Include(t => t.ServiceCategory)
                .FirstOrDefaultAsync(t => t.Id == id);
            
            if (treatment == null) return NotFound();

            return View(treatment);
        }

        public IActionResult Procedures() => RedirectToAction("Services");

        public async Task<IActionResult> Gallery(string category = "All")
        {
            var galleryItems = await _context.GalleryItems.ToListAsync();
            ViewBag.ActiveCategory = category;
            var filtered = category == "All" ? galleryItems : galleryItems.Where(i => i.Category == category).ToList();
            return View(filtered);
        }

        public async Task<IActionResult> Videos()
        {
            var videos = await _context.VideoItems.ToListAsync();
            return View(videos);
        }

        public IActionResult FAQ() => View();
        public async Task<IActionResult> Contact()
        {
            ViewBag.Treatments = await _context.Treatments.OrderBy(t => t.Name).ToListAsync();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitConsultation(Consultation consultation)
        {
            if (ModelState.IsValid)
            {
                _context.Consultations.Add(consultation);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Votre demande a été envoyée avec succès. Nous vous contacterons bientôt.";
                return RedirectToAction("Contact");
            }
            ViewBag.Treatments = await _context.Treatments.OrderBy(t => t.Name).ToListAsync();
            return View("Contact", consultation);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitWAEnquiry(string gender, string zones)
        {
            try
            {
                var consultation = new Consultation
                {
                    FullName = "Patient WhatsApp (" + (gender == "female" ? "Femme" : "Homme") + ")",
                    Email = "whatsapp@drkchida.com",
                    Phone = "WhatsApp",
                    Subject = "Sélecteur de zones faciales",
                    Message = "Zones sélectionnées : " + zones,
                    SubmittedAt = DateTime.Now,
                    IsRead = false
                };

                _context.Consultations.Add(consultation);
                await _context.SaveChangesAsync();

                return Json(new { success = true, id = consultation.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName,
                Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.MakeCookieValue(new Microsoft.AspNetCore.Localization.RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
