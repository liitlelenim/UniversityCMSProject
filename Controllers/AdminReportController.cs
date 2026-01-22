using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.Models;

namespace UniversityCMSProject.Controllers;

[Authorize(Roles = "Admin")]
public class AdminReportController : Controller
{
    private readonly AppDbContext _context;

    public AdminReportController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var reports = await _context.Reports
            .Include(r => r.Page)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
        return View(reports);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var report = await _context.Reports
            .Include(r => r.Page)
            .FirstOrDefaultAsync(r => r.Id == id);
        if (report == null) return NotFound();
        return View(report);
    }

    [HttpPost]
    public async Task<IActionResult> MarkResolved(int id)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report == null) return NotFound();
        report.IsResolved = true;
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Report marked as resolved.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var report = await _context.Reports.FindAsync(id);
        if (report != null)
        {
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Report deleted.";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeletePage(int id)
    {
        var report = await _context.Reports
            .Include(r => r.Page)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (report?.Page != null)
        {
            var pageReports = await _context.Reports
                .Where(r => r.PageId == report.Page.Id)
                .ToListAsync();
            _context.Reports.RemoveRange(pageReports);
            _context.UserPages.Remove(report.Page);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Page and all related reports deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}