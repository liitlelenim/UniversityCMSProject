using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.DbContext;

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

            if (page == null) return NotFound();

            var comments = await _context.Comments
                .Where(c => c.Page.Id == page.Id)
                .Include(c => c.Author)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            ViewBag.Comments = comments;
            ViewBag.PageId = page.Id;

            return View(page);
        }
    }
}