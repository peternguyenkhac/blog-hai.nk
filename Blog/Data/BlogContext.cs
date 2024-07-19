using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class BlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.Position).HasConversion(
                    p => string.Join(',', p),
                    p => p.Split(',', StringSplitOptions.RemoveEmptyEntries));
            });
               
        }
    }
}
