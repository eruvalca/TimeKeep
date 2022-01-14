using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeKeep.Server.Data.Configuration;
using TimeKeep.Shared.Models;

namespace TimeKeep.Server.Data
{
    public class AppDbContext : IdentityDbContext<TimeKeepUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PTOEntry> PTOEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
