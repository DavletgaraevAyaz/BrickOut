using BrickOutApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BrickOutApi.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
    }
}
