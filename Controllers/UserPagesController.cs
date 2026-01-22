using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.DbContext;
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
            var isAdmin = User.IsInRole("Admin");

            if (page.UserId != userId && !isAdmin)
                return Forbid();

            return View(page);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UserPage page)
        {
            if (id != page.Id) return NotFound();

            var urlExists = await context.UserPages
                .AnyAsync(p => p.Url == page.Url && p.Id != id);

            var originalPage = await context.UserPages.FindAsync(id);
            if (originalPage == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (originalPage.UserId != userId && !isAdmin)
                return Forbid();

            if (urlExists)
            {
                ModelState.AddModelError("Url", "URL already taken");
            }

            if (ModelState.IsValid)
            {
                originalPage.Title = page.Title;
                originalPage.Url = page.Url;
                originalPage.HtmlContent = page.HtmlContent;
                originalPage.CssContent = page.CssContent;
                originalPage.JsContent = page.JsContent;
                originalPage.IsPublic = page.IsPublic;

                try
                {
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
            var isAdmin = User.IsInRole("Admin");

            if (page.UserId != userId && !isAdmin)
                return Forbid();

            return View(page);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await context.UserPages.FindAsync(id);
            if (page == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (page.UserId != userId && !isAdmin)
                return Forbid();

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