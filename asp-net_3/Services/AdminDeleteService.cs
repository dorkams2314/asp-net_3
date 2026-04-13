using asp_net_3.Data;
using asp_net_3.Models;
using Microsoft.EntityFrameworkCore;

namespace asp_net_3.Services {
    public class AdminDeleteService {
        private readonly ApplicationDbContext _context;

        public AdminDeleteService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<bool> DeleteRoleAsync(int id) {
            Role? role = await _context.Roles.FindAsync(id);
            if (role == null)
                return false;

            List<int> userIds = await _context.Users
                .Where(x => x.RoleId == id)
                .Select(x => x.Id)
                .ToListAsync();

            if (userIds.Any())
                await DeleteUsersByIdsAsync(userIds);

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id) {
            User? user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            await DeleteUsersByIdsAsync(new List<int> { id });
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id) {
            Category? category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            List<int> productIds = await _context.Products
                .Where(x => x.CategoryId == id)
                .Select(x => x.Id)
                .ToListAsync();

            if (productIds.Any())
                await DeleteProductsByIdsAsync(productIds);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id) {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            await DeleteProductsByIdsAsync(new List<int> { id });
            return true;
        }

        public async Task<bool> DeleteCartAsync(int id) {
            Cart? cart = await _context.Carts.FindAsync(id);
            if (cart == null)
                return false;

            List<CartItem> cartItems = await _context.CartItems
                .Where(x => x.CartId == id)
                .ToListAsync();

            if (cartItems.Any())
                _context.CartItems.RemoveRange(cartItems);

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id) {
            Order? order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            List<OrderItem> orderItems = await _context.OrderItems
                .Where(x => x.OrderId == id)
                .ToListAsync();

            if (orderItems.Any())
                _context.OrderItems.RemoveRange(orderItems);

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task DeleteUsersByIdsAsync(List<int> userIds) {
            List<int> cartIds = await _context.Carts
                .Where(x => userIds.Contains(x.UserId))
                .Select(x => x.Id)
                .ToListAsync();

            List<int> orderIds = await _context.Orders
                .Where(x => userIds.Contains(x.UserId))
                .Select(x => x.Id)
                .ToListAsync();

            if (cartIds.Any()) {
                List<CartItem> cartItems = await _context.CartItems
                    .Where(x => cartIds.Contains(x.CartId))
                    .ToListAsync();

                if (cartItems.Any())
                    _context.CartItems.RemoveRange(cartItems);

                List<Cart> carts = await _context.Carts
                    .Where(x => cartIds.Contains(x.Id))
                    .ToListAsync();

                if (carts.Any())
                    _context.Carts.RemoveRange(carts);
            }

            if (orderIds.Any()) {
                List<OrderItem> orderItems = await _context.OrderItems
                    .Where(x => orderIds.Contains(x.OrderId))
                    .ToListAsync();

                if (orderItems.Any())
                    _context.OrderItems.RemoveRange(orderItems);

                List<Order> orders = await _context.Orders
                    .Where(x => orderIds.Contains(x.Id))
                    .ToListAsync();

                if (orders.Any())
                    _context.Orders.RemoveRange(orders);
            }

            List<User> users = await _context.Users
                .Where(x => userIds.Contains(x.Id))
                .ToListAsync();

            if (users.Any())
                _context.Users.RemoveRange(users);

            await _context.SaveChangesAsync();
        }

        private async Task DeleteProductsByIdsAsync(List<int> productIds) {
            List<CartItem> cartItems = await _context.CartItems
                .Where(x => productIds.Contains(x.ProductId))
                .ToListAsync();

            if (cartItems.Any())
                _context.CartItems.RemoveRange(cartItems);

            List<OrderItem> orderItems = await _context.OrderItems
                .Where(x => productIds.Contains(x.ProductId))
                .ToListAsync();

            if (orderItems.Any())
                _context.OrderItems.RemoveRange(orderItems);

            List<Product> products = await _context.Products
                .Where(x => productIds.Contains(x.Id))
                .ToListAsync();

            if (products.Any())
                _context.Products.RemoveRange(products);

            await _context.SaveChangesAsync();
        }
    }
}
