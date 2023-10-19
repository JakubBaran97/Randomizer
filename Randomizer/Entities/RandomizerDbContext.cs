using Microsoft.EntityFrameworkCore;
using Randomizer.Models;

namespace Randomizer.Entities
{
    public class RandomizerDbContext : DbContext
    {
        private string _connectionString = "server=(localdb)\\local;database=RandomizerDb;Trusted_connection=true";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<User> User { get; set; }
        
       


    }
}
