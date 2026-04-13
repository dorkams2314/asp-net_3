using System.ComponentModel.DataAnnotations;

namespace asp_net_3.ValidationAttributes {
    public class ProductNameNoDigitsAttribute : ValidationAttribute {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            string? text = value as string;

            if (string.IsNullOrWhiteSpace(text))
                return ValidationResult.Success;

            foreach (char symbol in text) {
                if (char.IsDigit(symbol))
                    return new ValidationResult("Название товара не должно содержать цифры.");
            }

            return ValidationResult.Success;
        }
    }
}
