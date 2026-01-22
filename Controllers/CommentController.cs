using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.DbContext;
using UniversityCMSProject.Models;

namespace UniversityCMSProject.Controllers;

[Authorize]
public class CommentController(AppDbContext context) : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int pageId, string content)
    {
        var page = await context.UserPages.FindAsync(pageId);
        if (page == null) return NotFound();

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Forbid();

        var user = await context.Users.FindAsync(userId);
        if (user == null) return Forbid();

        var comment = new Comment
        {
            Page = page,
            Author = user,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        context.Comments.Add(comment);
        await context.SaveChangesAsync();

        return RedirectToAction("ViewPage", "Public", new { url = page.Url });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await context.Comments
            .Include(c => c.Page)
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == id);
            
        if (comment == null) return NotFound();

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        if (comment.Author.Id != userId && !isAdmin)
            return Forbid();

        context.Comments.Remove(comment);
        await context.SaveChangesAsync();

        return RedirectToAction("ViewPage", "Public", new { url = comment.Page?.Url });
    }
}