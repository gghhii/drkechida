using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DrKchida.Data;
using DrKchida.Models;
using System.Threading.Tasks;

namespace DrKchida.Controllers
{
    [Authorize(AuthenticationSchemes = "CookieAuth")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TreatmentsCount = await _context.Treatments.CountAsync();
            ViewBag.GalleryCount = await _context.GalleryItems.CountAsync();
            ViewBag.VideosCount = await _context.VideoItems.CountAsync();
            ViewBag.ConsultationsCount = await _context.Consultations.CountAsync(c => !c.IsRead);
            return View();
        }

        // --- TREATMENTS (SOINS) ---
        public async Task<IActionResult> Treatments()
        {
            var treatments = await _context.Treatments.Include(t => t.ServiceCategory).ToListAsync();
            return View(treatments);
        }

        public async Task<IActionResult> CreateTreatment()
        {
            ViewBag.Categories = await _context.ServiceCategories.ToListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTreatment(Treatment treatment, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "treatments");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                treatment.ImageUrl = $"/images/treatments/{uniqueFileName}";
            }

            if (ModelState.IsValid)
            {
                _context.Add(treatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Treatments));
            }
            ViewBag.Categories = await _context.ServiceCategories.ToListAsync();
            return View(treatment);
        }

        public async Task<IActionResult> EditTreatment(int? id)
        {
            if (id == null) return NotFound();
            var treatment = await _context.Treatments.FindAsync(id);
            if (treatment == null) return NotFound();
            var categories = await _context.ServiceCategories.ToListAsync();
            ViewBag.Categories = categories;

            // Normalize SubCategory: find the exact spelling from AllowedSubCategories
            // This fixes cases where the saved value has a slightly different spelling
            if (!string.IsNullOrEmpty(treatment.SubCategory))
            {
                var category = categories.FirstOrDefault(c => c.Id == treatment.ServiceCategoryId);
                if (category != null && !string.IsNullOrEmpty(category.AllowedSubCategories))
                {
                    var allowed = category.AllowedSubCategories
                        .Split(';')
                        .Select(s => s.Trim())
                        .Where(s => !string.IsNullOrEmpty(s))
                        .ToList();

                    // Find the best match (case-insensitive)
                    var match = allowed.FirstOrDefault(s =>
                        string.Equals(s, treatment.SubCategory.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (match != null)
                    {
                        // Correct the SubCategory to the exact spelling in the allowed list
                        treatment.SubCategory = match;
                    }
                }
            }

            return View(treatment);
        }

        [HttpPost]
        public async Task<IActionResult> EditTreatment(Treatment treatment, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var existingTreatment = await _context.Treatments.AsNoTracking().FirstOrDefaultAsync(t => t.Id == treatment.Id);

                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "treatments");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    treatment.ImageUrl = $"/images/treatments/{uniqueFileName}";
                }
                else if (existingTreatment != null)
                {
                    treatment.ImageUrl = existingTreatment.ImageUrl;
                }

                _context.Update(treatment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Treatments));
            }
            ViewBag.Categories = await _context.ServiceCategories.ToListAsync();
            return View(treatment);
        }

        public async Task<IActionResult> DeleteTreatment(int id)
        {
            var treatment = await _context.Treatments.FindAsync(id);
            if (treatment != null)
            {
                _context.Treatments.Remove(treatment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Treatments));
        }

        // --- GALLERY ---
        public async Task<IActionResult> Gallery()
        {
            var items = await _context.GalleryItems.ToListAsync();
            return View(items);
        }

        public IActionResult CreateGalleryItem() => View();

        [HttpPost]
        public async Task<IActionResult> CreateGalleryItem(GalleryItem item, IFormFile? imageBefore, IFormFile? imageAfter)
        {
            if (ModelState.IsValid)
            {
                // Handle Before Image Upload
                if (imageBefore != null && imageBefore.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "gallery");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    var uniqueFileName = $"{Guid.NewGuid()}_{imageBefore.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageBefore.CopyToAsync(fileStream);
                    }
                    
                    item.ImageBefore = $"images/gallery/{uniqueFileName}";
                }
                
                // Handle After Image Upload
                if (imageAfter != null && imageAfter.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "gallery");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    var uniqueFileName = $"{Guid.NewGuid()}_{imageAfter.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageAfter.CopyToAsync(fileStream);
                    }
                    
                    item.ImageAfter = $"images/gallery/{uniqueFileName}";
                }
                
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Gallery));
            }
            return View(item);
        }

        public async Task<IActionResult> EditGalleryItem(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.GalleryItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> EditGalleryItem(GalleryItem item, IFormFile? imageBefore, IFormFile? imageAfter)
        {
            if (ModelState.IsValid)
            {
                var existingItem = await _context.GalleryItems.AsNoTracking().FirstOrDefaultAsync(g => g.Id == item.Id);
                
                // Handle Before Image Upload (only if new file provided)
                if (imageBefore != null && imageBefore.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "gallery");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    var uniqueFileName = $"{Guid.NewGuid()}_{imageBefore.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageBefore.CopyToAsync(fileStream);
                    }
                    
                    item.ImageBefore = $"images/gallery/{uniqueFileName}";
                }
                else if (existingItem != null)
                {
                    item.ImageBefore = existingItem.ImageBefore;
                }
                
                // Handle After Image Upload (only if new file provided)
                if (imageAfter != null && imageAfter.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "gallery");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    var uniqueFileName = $"{Guid.NewGuid()}_{imageAfter.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageAfter.CopyToAsync(fileStream);
                    }
                    
                    item.ImageAfter = $"images/gallery/{uniqueFileName}";
                }
                else if (existingItem != null)
                {
                    item.ImageAfter = existingItem.ImageAfter;
                }
                
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Gallery));
            }
            return View(item);
        }

        public async Task<IActionResult> DeleteGalleryItem(int id)
        {
            var item = await _context.GalleryItems.FindAsync(id);
            if (item != null)
            {
                _context.GalleryItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Gallery));
        }

        // --- VIDEOS ---
        public async Task<IActionResult> Videos()
        {
            var videos = await _context.VideoItems.ToListAsync();
            return View(videos);
        }

        public IActionResult CreateVideo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVideo(VideoItem video, IFormFile? videoFile, IFormFile? thumbnailFile)
        {
            // Handle Video File Upload
            if (videoFile != null && videoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "videos");
                Directory.CreateDirectory(uploadsFolder);
                
                var uniqueFileName = $"{Guid.NewGuid()}_{videoFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(fileStream);
                }
                
                video.VideoUrl = $"/videos/{uniqueFileName}";
                // Clear validation error since we just set the URL
                ModelState.Remove(nameof(video.VideoUrl));
            }

            // Handle Thumbnail Upload
            if (thumbnailFile != null && thumbnailFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "videos");
                Directory.CreateDirectory(uploadsFolder);
                
                var uniqueFileName = $"{Guid.NewGuid()}_{thumbnailFile.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await thumbnailFile.CopyToAsync(fileStream);
                }
                
                video.ThumbnailUrl = $"/images/videos/{uniqueFileName}";
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(video.VideoUrl))
                {
                    ModelState.AddModelError(nameof(video.VideoUrl), "Veuillez télécharger une vidéo ou fournir un lien.");
                    return View(video);
                }

                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Videos));
            }
            
            return View(video);
        }

        public async Task<IActionResult> EditVideo(int? id)
        {
            if (id == null) return NotFound();
            var video = await _context.VideoItems.FindAsync(id);
            if (video == null) return NotFound();
            return View(video);
        }

        [HttpPost]
        public async Task<IActionResult> EditVideo(VideoItem video, IFormFile? videoFile, IFormFile? thumbnailFile)
        {
            if (ModelState.IsValid)
            {
                var existingVideo = await _context.VideoItems.AsNoTracking().FirstOrDefaultAsync(v => v.Id == video.Id);

                // Handle Video File Update
                if (videoFile != null && videoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "videos");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = $"{Guid.NewGuid()}_{videoFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await videoFile.CopyToAsync(fileStream);
                    }
                    video.VideoUrl = $"/videos/{uniqueFileName}";
                }
                else if (existingVideo != null)
                {
                    video.VideoUrl = existingVideo.VideoUrl;
                }

                // Handle Thumbnail Update
                if (thumbnailFile != null && thumbnailFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "videos");
                    Directory.CreateDirectory(uploadsFolder);
                    var uniqueFileName = $"{Guid.NewGuid()}_{thumbnailFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnailFile.CopyToAsync(fileStream);
                    }
                    video.ThumbnailUrl = $"/images/videos/{uniqueFileName}";
                }
                else if (existingVideo != null)
                {
                    video.ThumbnailUrl = existingVideo.ThumbnailUrl;
                }

                _context.Update(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Videos));
            }
            return View(video);
        }

        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await _context.VideoItems.FindAsync(id);
            if (video != null)
            {
                _context.VideoItems.Remove(video);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Videos));
        }

        // --- CONSULTATIONS ---
        public async Task<IActionResult> Consultations()
        {
            var consultations = await _context.Consultations
                .OrderByDescending(c => c.SubmittedAt)
                .ToListAsync();
            return View(consultations);
        }

        public async Task<IActionResult> ConsultationDetails(int id)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null) return NotFound();

            if (!consultation.IsRead)
            {
                consultation.IsRead = true;
                await _context.SaveChangesAsync();
            }

            return View(consultation);
        }

        public async Task<IActionResult> MarkAsRead(int id)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation != null)
            {
                consultation.IsRead = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Consultations));
        }

        public async Task<IActionResult> DeleteConsultation(int id)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation != null)
            {
                _context.Consultations.Remove(consultation);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Consultations));
        }
    }
}
