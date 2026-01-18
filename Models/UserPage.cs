using System.ComponentModel.DataAnnotations;

namespace UniversityCMSProject.Models;

public class UserPage
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL is required")]
    [StringLength(50, ErrorMessage = "URL cannot be longer than 50 characters")]
    [RegularExpression(@"^[a-z0-9\-]+$", 
        ErrorMessage = "URL can only contain lowercase letters, numbers and hyphens")]
    public string Url { get; set; } = string.Empty;

    [Required(ErrorMessage = "HTML content is required")]
    [MinLength(10, ErrorMessage = "HTML content must be at least 10 characters")]
    public string HtmlContent { get; set; } = string.Empty;

    public string? CssContent { get; set; } = string.Empty;
    public string? JsContent { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublic { get; set; } = true;
    
    public string UserId { get; set; } = string.Empty;
    
    public AppUser? User { get; set; }

    public List<PageTag> Tags { get; set; } = new List<PageTag>();
}