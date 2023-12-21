using Microsoft.EntityFrameworkCore;
using Ozbul.Application.Portal.Repository.Entities;

namespace Ozbul.Application.Portal.Repository;
public class AppDbContext : DbContext
{

    public DbSet<Student> Students { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<User> Users { get; set; }  

    public AppDbContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=Application.Portal;Trusted_Connection=True;MultipleActiveResultSets=true"); 


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }


    
}