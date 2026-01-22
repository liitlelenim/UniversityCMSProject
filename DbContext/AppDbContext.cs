using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityCMSProject.Models;

namespace UniversityCMSProject.DbContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<UserPage> UserPages { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Comment> Comments { get; set; }

}