using System.ComponentModel.DataAnnotations;
using asp_net_3.Models;

namespace asp_net_3.ValidationAttributes {
    public class ProductPriceByCategoryAttribute : ValidationAttribute {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            if (value == null)
                return new ValidationResult("Цена товара обязательна для заполнения.");

            Product? product = validationContext.ObjectInstance as Product;
            if (product == null)
                return ValidationResult.Success;

            decimal price = Convert.ToDecimal(value);
            decimal minPrice = 0;

            if (product.CategoryId == 1)
                minPrice = 500;
            else if (product.CategoryId == 2)
                minPrice = 700;
            else if (product.CategoryId == 3)
                minPrice = 1000;
            else if (product.CategoryId == 4)
                minPrice = 300;

            if (price < minPrice)
                return new ValidationResult("Для выбранной категории цена должна быть не меньше " + minPrice + " рублей.");

            return ValidationResult.Success;
        }
    }
}
