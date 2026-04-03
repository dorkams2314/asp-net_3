using asp_net_3.Models;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Cart>().ToTable("Carts");
            modelBuilder.Entity<CartItem>().ToTable("CartItems");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");

            modelBuilder.Entity<Product>().Property(product => product.Name).HasMaxLength(200);
            modelBuilder.Entity<Product>().Property(product => product.Description).HasMaxLength(500);
            modelBuilder.Entity<Product>().Property(product => product.Price).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Category>().Property(category => category.Name).HasMaxLength(100);
            modelBuilder.Entity<Role>().Property(role => role.Name).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(user => user.Login).HasMaxLength(100);
            modelBuilder.Entity<User>().Property(user => user.PasswordHash).HasMaxLength(256);
            modelBuilder.Entity<OrderItem>().Property(orderItem => orderItem.Price).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<User>()
                .HasOne(user => user.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cart>()
                .HasOne(cart => cart.User)
                .WithMany(user => user.Carts)
                .HasForeignKey(cart => cart.UserId)
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<Order>()
                .HasOne(order => order.User)
                .WithMany(user => user.Orders)
                .HasForeignKey(order => order.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.Items)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.Product)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
