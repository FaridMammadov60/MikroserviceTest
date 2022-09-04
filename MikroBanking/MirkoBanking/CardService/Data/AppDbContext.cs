using CardService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Card> Cards { get; set; }
    }
}
