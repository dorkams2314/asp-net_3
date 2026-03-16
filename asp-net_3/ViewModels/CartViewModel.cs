namespace asp_net_3.ViewModels {
    public class CartViewModel {
        public List<CartItemViewModel> Items { get; set; }

        public decimal TotalPrice {
            get {
                decimal totalPrice = 0;

                foreach (var item in Items)
                    totalPrice = totalPrice + item.TotalPrice;

                return totalPrice;
            }
        }

        public CartViewModel() {
            Items = new List<CartItemViewModel>();
        }
    }
}
