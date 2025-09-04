using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EagerLoadingWebAPI.Models
{
    public class AppDbContext : DbContext // Ensure AppDbContext inherits from DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        } // Fix CS0311 and CS1729 by inheriting from DbContext

        public DbSet<Library> Libraries { get; set; } = null!; // Fix CS8618 by initializing with null-forgiving operator
        public DbSet<Book> Books { get; set; } = null!; // Fix CS8618 by initializing with null-forgiving operator

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent config (optional – attributes already cover basics)
            modelBuilder.Entity<Library>()
                .HasMany(l => l.Books)
                .WithOne(b => b.Library!)
                .HasForeignKey(b => b.LibraryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
