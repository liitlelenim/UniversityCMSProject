using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.Models;

namespace UniversityCMSProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Dashboard()
        {
            return View();
        }
        
        public async Task<IActionResult> AllPages()
        {
            var pages = await _context.UserPages
                .Include(p => p.User)
                .ToListAsync();
            return View(pages);
        }
        
        public async Task<IActionResult> EditPage(int id)
        {
            var page = await _context.UserPages
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
                
            if (page == null) return NotFound();
            
            return View(page);
        }
        
        [HttpPost]
        public async Task<IActionResult> EditPage(int id, UserPage updatedPage)
        {
            var page = await _context.UserPages.FindAsync(id);
            if (page == null) return NotFound();
            
            page.Title = updatedPage.Title;
            page.Url = updatedPage.Url;
            page.HtmlContent = updatedPage.HtmlContent;
            page.IsPublic = updatedPage.IsPublic;
            
            await _context.SaveChangesAsync();
            return RedirectToAction("AllPages");
        }
        
        [HttpPost]
        public async Task<IActionResult> DeletePage(int id)
        {
            var page = await _context.UserPages.FindAsync(id);
            if (page != null)
            {
                _context.UserPages.Remove(page);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("AllPages");
        }
    }
}