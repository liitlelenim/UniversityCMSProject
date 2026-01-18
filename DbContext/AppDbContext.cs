using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.Models;

namespace UniversityCMSProject;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<UserPage> UserPages { get; set; }
    public DbSet<PageTag> PageTags { get; set; }
}