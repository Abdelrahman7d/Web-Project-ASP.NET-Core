using Entity;
using Microsoft.EntityFrameworkCore;

namespace Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Clinic> Clinics { get; set; }

}