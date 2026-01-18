using System.ComponentModel.DataAnnotations;

namespace UniversityCMSProject.Models;

public class PageTag
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tag name is required")]
    [StringLength(30, ErrorMessage = "Tag name must be less than 30 characters")]
    [RegularExpression(@"^[a-zA-Z0-9\-]+$",
        ErrorMessage = "Tag can only contain letters, numbers and hyphens")]
    public string Name { get; set; }
    
    public ICollection<UserPage> Pages { get; set; } = new List<UserPage>();
}