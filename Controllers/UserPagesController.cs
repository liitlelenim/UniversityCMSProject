using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.Models;

namespace UniversityCMSProject.Controllers
{
    [Authorize]
    public class UserPagesController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var pages = await context.UserPages
                .Where(p => p.UserId == userId)
                .ToListAsync();

            return View(pages);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserPage page)
        {
            var urlExists = await context.UserPages
                .AnyAsync(p => p.Url == page.Url);

            if (urlExists)
            {
                ModelState.AddModelError("Url", "This URL is already taken");
            }

            page.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TryValidateModel(page);

            if (ModelState.IsValid)
            {
                page.CreatedAt = DateTime.UtcNow;

                context.Add(page);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(page);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var page = await context.UserPages.FindAsync(id);
            if (page == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId) return Forbid();

            return View(page);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserPage page)
        {
            
            if (id != page.Id) return NotFound();
            var urlExists = await context.UserPages
                .AnyAsync(p => p.Url == page.Url && p.Id != id);
            
            page.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            TryValidateModel(page);
               
            if (urlExists)
            {
                ModelState.AddModelError("Url", "URL already taken");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(page);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.Id)) return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(page);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var page = await context.UserPages.FindAsync(id);
            if (page == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (page.UserId != userId) return Forbid();

            return View(page);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await context.UserPages.FindAsync(id);
            if (page != null)
            {
                context.UserPages.Remove(page);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PageExists(int id)
        {
            return context.UserPages.Any(e => e.Id == id);
        }
    }
}