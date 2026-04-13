using System.ComponentModel.DataAnnotations;

namespace asp_net_3.ValidationAttributes {
    public class ProductDescriptionMinWordsAttribute : ValidationAttribute {
        private readonly int _minWords;

        public ProductDescriptionMinWordsAttribute(int minWords) {
            _minWords = minWords;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            string? text = value as string;

            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult("Описание товара обязательно для заполнения.");

            string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < _minWords)
                return new ValidationResult("Описание товара должно содержать не меньше " + _minWords + " слов.");

            return ValidationResult.Success;
        }
    }
}
