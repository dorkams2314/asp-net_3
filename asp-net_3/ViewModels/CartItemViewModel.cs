namespace asp_net_3.ViewModels {
    public class CartItemViewModel {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice {
            get { return Price * Quantity; }
        }

        public CartItemViewModel() {
            ProductName = "";
            Description = "";
            CategoryName = "";
        }
    }
}
