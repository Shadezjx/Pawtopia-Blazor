using Microsoft.EntityFrameworkCore;
using Pawtopia.Models;

namespace Pawtopia.Data
{
    // Kế thừa DbContext thường, không dùng IdentityDbContext nữa
    public class PawtopiaDbContext : DbContext
    {
        public PawtopiaDbContext(DbContextOptions<PawtopiaDbContext> options)
            : base(options)
        {
        }

        // Khai báo bảng của riêng bạn ở đây
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình bảng User của bạn (ví dụ đặt tên bảng là 'MyUsers')
            modelBuilder.Entity<User>().ToTable("Users");
            base.OnModelCreating(modelBuilder);
        }
    }
}