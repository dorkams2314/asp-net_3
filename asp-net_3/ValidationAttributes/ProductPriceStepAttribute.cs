using System.ComponentModel.DataAnnotations;

namespace asp_net_3.ValidationAttributes {
    public class ProductPriceStepAttribute : ValidationAttribute {
        private readonly decimal _step;

        public ProductPriceStepAttribute(double step) {
            _step = Convert.ToDecimal(step);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
            if (value == null)
                return new ValidationResult("Цена товара обязательна для заполнения.");

            decimal price = Convert.ToDecimal(value);

            if (price <= 0)
                return new ValidationResult("Цена товара должна быть больше нуля.");

            decimal remainder = price % _step;

            if (remainder != 0)
                return new ValidationResult("Цена товара должна быть кратна " + _step + " рублям.");

            return ValidationResult.Success;
        }
    }
}
