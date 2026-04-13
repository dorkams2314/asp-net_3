using System.ComponentModel.DataAnnotations;

namespace asp_net_3.ValidationAttributes {
    public class ProductNameUppercaseAttribute : ValidationAttribute {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            string? text = value as string;

            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult("Название товара обязательно для заполнения.");

            string trimmedText = text.Trim();
            char firstChar = trimmedText[0];

            if (!char.IsLetter(firstChar) || !char.IsUpper(firstChar))
                return new ValidationResult("Название товара должно начинаться с заглавной буквы.");

            return ValidationResult.Success;
        }
    }
}
