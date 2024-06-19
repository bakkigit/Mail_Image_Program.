using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MailimageProgram
{
    public class ImageContext : DbContext
    {
        public DbSet<ImageEntity> ImageTable { get; set; }
        string connectionString = "";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageEntity>()
                .HasKey(e => e.ImageID);
        }
    }
}
