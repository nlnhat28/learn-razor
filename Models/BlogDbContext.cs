using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RAZOR_EF.Models
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Article> Article { get; set; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}