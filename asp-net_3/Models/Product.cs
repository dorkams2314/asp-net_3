using asp_net_3.ValidationAttributes;

namespace asp_net_3.Models {
    public class Product {
        public int Id { get; set; }

        [ProductNameUppercase]
        [ProductNameNoDigits]
        [ProductNameNoDoubleSpaces]
        public string Name { get; set; }

        [ProductDescriptionMinWords(4)]
        public string Description { get; set; }

        [ProductPriceStep(50)]
        [ProductPriceByCategory]
        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public Product() {
            Name = "";
            Description = "";
        }
    }
}
