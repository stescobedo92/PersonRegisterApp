using Microsoft.EntityFrameworkCore;

namespace PersonRegisterApp.Web.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Person> People { get; set; }
}
