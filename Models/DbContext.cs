using Microsoft.EntityFrameworkCore;

namespace LoginApi.Models
{
    public class UserDbContext : DbContext
    {
       public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("users");
            modelBuilder.Entity<Users>().Property(u => u.DateRegistered).HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Users>()
            .HasIndex(u => u.Username)
            .IsUnique();

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.SetTableName(entity.GetTableName().ToLower());

                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToLower());
                }
            }
            base.OnModelCreating(modelBuilder);
        }

    }
}
