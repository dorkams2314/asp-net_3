namespace asp_net_3.Models {
    public class Cart {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public List<CartItem> Items { get; set; }

        public Cart() {
            Items = new List<CartItem>();
        }
    }
}
