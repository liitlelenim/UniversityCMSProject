using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityCMSProject.Models;

public class Report
{
    public int Id { get; set; }
    public int PageId { get; set; }
    public UserPage? Page { get; set; }
    [Required]
    [StringLength(500)]
    public string Text { get; set; } = string.Empty;
    [Required]
    public string ReporterNick { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsResolved { get; set; } = false;
}