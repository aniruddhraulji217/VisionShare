using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using VisionShare.Data;
using VisionShare.Models;
using VisionShare.Models.ViewModels;

namespace VisionShare.Controllers
{
    public class IdeaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly string[] _allowedExtention = { ".jpg", ".jpeg", ".png" };

        public IdeaController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // ------------------------------
        // SHOW ALL IDEAS
        // ------------------------------
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ideas = await _context.Ideas
                .Include(i => i.Upvotes)
                .OrderByDescending(i => i.DatePosted)
                .ToListAsync();

            // List of ideas liked by current user
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);

                ViewBag.LikedIdeaIds = await _context.IdeaUpvotes
                    .Where(x => x.UserId == userId)
                    .Select(x => x.IdeaId)
                    .ToListAsync();
            }
            else
            {
                ViewBag.LikedIdeaIds = new List<int>();
            }

            return View(ideas);
        }

        // ------------------------------
        // CREATE IDEA FORM (LOGIN REQUIRED)
        // ------------------------------
        [HttpGet]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            return View(new IdeaViewModel());
        }

        // ------------------------------
        // CREATE IDEA
        // ------------------------------
        [HttpPost]
        public async Task<IActionResult> Create(IdeaViewModel ideaViewModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Remove validation for fields that will be set manually
            ModelState.Remove("Idea.FeatureImagePath");
            ModelState.Remove("Idea.UserId");

            if (!ModelState.IsValid)
                return View(ideaViewModel);

            // Validate file
            var ext = Path.GetExtension(ideaViewModel.FeatureImage.FileName).ToLower();
            if (!_allowedExtention.Contains(ext))
            {
                ModelState.AddModelError("", "Invalid file type! Only .jpg, .jpeg, .png allowed.");
                return View(ideaViewModel);
            }

            // Upload file
            string filePath = await UploadFileToFolder(ideaViewModel.FeatureImage);

            // Fill model AFTER validation
            ideaViewModel.Idea.FeatureImagePath = filePath;
            ideaViewModel.Idea.DatePosted = DateTime.Now;
            ideaViewModel.Idea.UpvoteCount = 0;
            ideaViewModel.Idea.ViewCount = 0;
            ideaViewModel.Idea.UserId = User.Identity.Name;

            _context.Ideas.Add(ideaViewModel.Idea);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyIdeas");
        }


        // ------------------------------
        // UPLOAD IMAGE
        // ------------------------------
        private async Task<string> UploadFileToFolder(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString() + extension;

            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string folderPath = Path.Combine(wwwRootPath, "images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fullPath = Path.Combine(folderPath, fileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/images/" + fileName;
        }

        // ------------------------------
        // DETAILS PAGE + INCREMENT VIEW
        // ------------------------------
        public async Task<IActionResult> Details(int id)
        {
            var idea = await _context.Ideas
                .Include(i => i.Upvotes)
                .FirstOrDefaultAsync(i => i.IdeaId == id);

            if (idea == null)
                return NotFound();

            // Increase view count
            idea.ViewCount++;
            _context.Ideas.Update(idea);
            await _context.SaveChangesAsync();

            // Check if current user already liked
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);

                ViewBag.UserLiked = await _context.IdeaUpvotes
                    .AnyAsync(x => x.IdeaId == id && x.UserId == userId);
            }
            else
            {
                ViewBag.UserLiked = false;
            }

            return View(idea);
        }

        // ------------------------------
        // LIKE / UNLIKE (TOGGLE)
        // ------------------------------
        [HttpPost]
        public async Task<IActionResult> ToggleLike(int ideaId)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false, message = "LOGIN_REQUIRED" });

            var userId = _userManager.GetUserId(User);

            var existing = await _context.IdeaUpvotes
                .FirstOrDefaultAsync(x => x.IdeaId == ideaId && x.UserId == userId);

            bool liked;

            if (existing != null)
            {
                // UNLIKE
                _context.IdeaUpvotes.Remove(existing);
                liked = false;
            }
            else
            {
                // LIKE
                await _context.IdeaUpvotes.AddAsync(new IdeaUpvote
                {
                    IdeaId = ideaId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                });
                liked = true;
            }

            await _context.SaveChangesAsync();

            // Update like count in Idea table
            var idea = await _context.Ideas.FindAsync(ideaId);
            idea.UpvoteCount = await _context.IdeaUpvotes.CountAsync(x => x.IdeaId == ideaId);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                liked = liked,
                likeCount = idea.UpvoteCount
            });
        }
        [HttpGet]
        public async Task<IActionResult> MyIdeas()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var userId = User.Identity.Name;

            var ideas = await _context.Ideas
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.DatePosted)
                .ToListAsync();

            return View(ideas);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);

            if (idea == null)
                return NotFound();

            // Only creator can delete
            if (idea.UserId != User.Identity.Name)
                return Unauthorized();

            _context.Ideas.Remove(idea);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyIdeas");
        }

        //EDIT IDEA FORM
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);

            if (idea == null)
                return NotFound();

            if (idea.UserId != User.Identity.Name)
                return Unauthorized();

            var vm = new IdeaViewModel
            {
                Idea = idea
            };

            return View("Create", vm);   // ← load the same Create.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, IdeaViewModel vm)
        {
            var idea = await _context.Ideas.FindAsync(id);

            if (idea == null)
                return NotFound();

            if (idea.UserId != User.Identity.Name)
                return Unauthorized();

            idea.Title = vm.Idea.Title;
            idea.Description = vm.Idea.Description;

            // If new image uploaded, replace old one
            if (vm.FeatureImage != null)
            {
                string path = await UploadFileToFolder(vm.FeatureImage);
                idea.FeatureImagePath = path;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("MyIdeas");
        }



    }
}
