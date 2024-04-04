using System.Globalization;
using System.Windows.Controls;

namespace PL.validation;
public class PositiveNumberValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (!(value is string stringValue))
            return new ValidationResult(false, "Value must be provided.");

        if (!double.TryParse(stringValue, NumberStyles.Any, cultureInfo, out double numberValue))
            return new ValidationResult(false, "Invalid number format.");

        if (numberValue < 0)
            return new ValidationResult(false, "Number must be positive.");
        return ValidationResult.ValidResult;
    }
}