using Microsoft.EntityFrameworkCore;

namespace UserApi.Model
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserInfo>? UserInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasNoKey();
                entity.ToTable("UserInfo");
                //entity.HasIndex(e => e.UserId).HasName("UserId").IsUnique();
                entity.Property(e => e.UserId).HasColumnName("UserId").IsUnicode(true);
                
                entity.Property(e => e.FirstName).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.LastName).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.UserName).HasMaxLength(30).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.marketingConsent).HasMaxLength(10).IsUnicode(false);

                entity.Property(e => e.CreatedDate).IsUnicode(false);
            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}