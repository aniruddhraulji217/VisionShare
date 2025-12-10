using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisionShare.Data;

namespace VisionShare.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ideas = await _context.Ideas
                .OrderByDescending(i => i.DatePosted)
                .ToListAsync();


            return View(ideas);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
