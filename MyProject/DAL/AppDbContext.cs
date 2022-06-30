using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
        }
        public DbSet<Person> Persons { get; set; }
    }
}
