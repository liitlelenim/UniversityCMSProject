using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UniversityCMSProject.Controllers
{
    public class PublicController : Controller
    {
        private readonly AppDbContext _context;

        public PublicController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ViewPage(string url)
        {
            var page = await _context.UserPages
                .FirstOrDefaultAsync(p => p.Url == url && p.IsPublic);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}