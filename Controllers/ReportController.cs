using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.DbContext;
using UniversityCMSProject.Models;

namespace UniversityCMSProject.Controllers;

public class ReportController : Controller
{
    private readonly AppDbContext _context;

    public ReportController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Create(int? pageId)
    {
        if (pageId == null) return NotFound();
        var page = await _context.UserPages.FindAsync(pageId);
        if (page == null) return NotFound();
        ViewBag.PageTitle = page.Title;
        ViewBag.PageId = pageId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int pageId, string text, string reporterNick = "Anonymous")
    {
        var page = await _context.UserPages.FindAsync(pageId);
        if (page == null) return NotFound();
        if (string.IsNullOrEmpty(reporterNick)) reporterNick = "Anonymous";

        var report = new Report
        {
            PageId = pageId,
            Text = text,
            ReporterNick = reporterNick,
            CreatedAt = DateTime.UtcNow,
            IsResolved = false
        };

        _context.Reports.Add(report);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Report submitted successfully.";
        return RedirectToAction("ViewPage", "Public", new { url = page.Url });
    }
}