using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VisionShare.Data;
using VisionShare.Models.ViewModels;

namespace VisionShare.Controllers
{
    public class IdeaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string[] _allowedExtention = { ".jpg", ".jpeg", "png" };

        public IdeaController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
           // var ideaViewModel = new IdeaViewModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdeaViewModel ideaViewModel)
        {
            if (ModelState.IsValid)
            {
                var inputFileExtention = Path.GetExtension(ideaViewModel.FeatureImage.FileName).ToLower();
                bool isAllowed = _allowedExtention.Contains(inputFileExtention);

                if (!isAllowed)
                {
                    ModelState.AddModelError("", "Invalid Image format.Aloowed format are .jpeg ,.jpg,.png");
                    return View(ideaViewModel);
                }

                ideaViewModel.Idea.FeatureImagePath = await UploadFiletoFolder(ideaViewModel.FeatureImage);

                // You may want to add code here to save the idea to the database, e.g.:
                // _context.Ideas.Add(ideaViewModel.Idea);
                // await _context.SaveChangesAsync();

                // Redirect to a suitable page after successful creation
                return RedirectToAction("Index"); // or another action/view
            }

            // If model state is not valid, return the view with the model
            return View(ideaViewModel);
        }

        private async Task<string> UploadFiletoFolder(IFormFile file)
        {
            var inputFileExtension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid().ToString() + inputFileExtension;
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var imagesFolderPath = Path.Combine(wwwRootPath, "images");

            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }
            var filePath = Path.Combine(imagesFolderPath, fileName);

            try
            {
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file", ex);
            }

            return "/images/" + fileName;
        }
    }
