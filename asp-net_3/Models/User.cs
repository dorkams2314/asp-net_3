namespace asp_net_3.Models {
    public class User {
        public int Id { get; set; }

        public string Login { get; set; }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }

        public Role? Role { get; set; }

        public List<Cart> Carts { get; set; }

        public List<Order> Orders { get; set; }

        public User() {
            Login = "";
            PasswordHash = "";
            Carts = new List<Cart>();
            Orders = new List<Order>();
        }
    }
}
