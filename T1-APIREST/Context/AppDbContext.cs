using Microsoft.EntityFrameworkCore;
using T1_APIREST.Models;

namespace T1_APIREST.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Film> Films { get; set; }

    }
}
