using System.ComponentModel.DataAnnotations;

namespace asp_net_3.ValidationAttributes {
    public class ProductNameNoDoubleSpacesAttribute : ValidationAttribute {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            string? text = value as string;

            if (string.IsNullOrWhiteSpace(text))
                return ValidationResult.Success;

            if (text.Contains("  "))
                return new ValidationResult("Название товара не должно содержать двойные пробелы.");

            return ValidationResult.Success;
        }
    }
}
