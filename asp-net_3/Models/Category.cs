namespace asp_net_3.Models {
    public class Category {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Product> Products { get; set; }

        public Category() {
            Name = "";
            Products = new List<Product>();
        }
    }
}
