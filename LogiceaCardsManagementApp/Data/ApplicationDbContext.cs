using LogiceaCardsManagementApp2.Models;
using Microsoft.EntityFrameworkCore;

namespace LogiceaCardsManagementApp2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> users { get; set; }
        public DbSet<Card> cards { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Card>(entity =>
            {
                entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
               
            });

            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                .IsRequired();
                entity.Property(e => e.Role)
               .IsRequired();
                entity.Property(e => e.Password)
               .IsRequired();

            });

            base.OnModelCreating(builder);
        }
    }
}
