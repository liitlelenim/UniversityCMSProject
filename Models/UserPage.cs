using System.ComponentModel.DataAnnotations;

namespace UniversityCMSProject.Models;

public class UserPage
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    [StringLength(50)]
    [RegularExpression(@"^[a-z0-9\-]+$")]
    public string Url { get; set; }

    [Required]
    [MinLength(10)]
    public string HtmlContent { get; set; }

    public string CssContent { get; set; }
    public string JsContent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublic { get; set; } = true;

    public string UserId { get; set; }
    public AppUser User { get; set; }

    public List<PageTag> Tags { get; set; } = new List<PageTag>();
}