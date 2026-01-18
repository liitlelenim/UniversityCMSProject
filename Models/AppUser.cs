using Microsoft.AspNetCore.Identity;

namespace UniversityCMSProject.Models;

public class AppUser : IdentityUser
{
    public string NickName { get; set; }
    public List<UserPage> Pages { get; set; }
}