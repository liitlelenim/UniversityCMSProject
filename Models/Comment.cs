using System.ComponentModel.DataAnnotations;

namespace UniversityCMSProject.Models;

public class Comment
{
    public int Id { get; set; }
    
    [Required]
    public UserPage Page { get; set; }
    
    [Required]
    public AppUser Author { get; set; }
    
    [Required]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}