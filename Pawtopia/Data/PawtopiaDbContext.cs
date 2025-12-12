using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pawtopia.Models;

namespace Pawtopia.Data
{
    public class PawtopiaDbContext(DbContextOptions<PawtopiaDbContext> options)
        : IdentityDbContext<
            User,
            IdentityRole,
            string, // PasswordHash
            IdentityUserClaim<string>,
            IdentityUserRole<string>,
            IdentityUserLogin<string>,
            IdentityRoleClaim<string>,
            IdentityUserToken<string>,
            IdentityUserPasskey<string>
        >(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ---
            // Renaming Identity tables
            // ---

            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("Logins");
            builder.Entity<IdentityUserToken<string>>().ToTable("Tokens");
            builder.Entity<IdentityUserPasskey<string>>().ToTable("Passkeys");

            // ---
            // Configuration navigation properties for many-to-many relationships
            // ---

            builder.Entity<User>()
                .HasMany(user => user.Roles)
                .WithMany()
                .UsingEntity<IdentityUserRole<string>>(
                    userRole => userRole
                        .HasOne<IdentityRole>()
                        .WithMany()
                        .HasForeignKey(userRole => userRole.RoleId)
                        .IsRequired(),
                    userRole => userRole
                        .HasOne<User>()
                        .WithMany(user => user.UserRoles)
                        .HasForeignKey(userRole => userRole.UserId)
                        .IsRequired()
                );

            // ---
            // Additional application configurations
            // ---

            // Seed roles
            var adminRoleId = "admin-role";
            var customerRoleId = "customer-role";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = customerRoleId,
                    ConcurrencyStamp = customerRoleId,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                }
            );
        }

        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductItem> ProductItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } = null!;
        public DbSet<Variation> Variations { get; set; } = null!;
        public DbSet<VariationOption> VariationOptions { get; set; } = null!;
    }
}
