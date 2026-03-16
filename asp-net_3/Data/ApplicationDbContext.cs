using asp_net_3.Models;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Cart>().ToTable("Carts");
            modelBuilder.Entity<CartItem>().ToTable("CartItems");

            modelBuilder.Entity<Product>().Property(product => product.Name).HasMaxLength(200);
            modelBuilder.Entity<Product>().Property(product => product.Description).HasMaxLength(500);
            modelBuilder.Entity<Product>().Property(product => product.Price).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Category>().Property(category => category.Name).HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(item => item.Cart)
                .WithMany(cart => cart.Items)
                .HasForeignKey(item => item.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(item => item.Product)
                .WithMany()
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
