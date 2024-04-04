using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PL.validation;
public class IdValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {

        if (!(value is string stringValue))
            return new ValidationResult(false, "Id must be provided.");

        if (!double.TryParse(stringValue, NumberStyles.Any, cultureInfo, out double numberValue))
            return new ValidationResult(false, "Invalid number format.");

        if (numberValue < 0)
            return new ValidationResult(false, "Id must be positive.");
        if (Regex.IsMatch(stringValue, @"^\d{6,}$"))
            return ValidationResult.ValidResult;

        return new ValidationResult(false, "Id must be with a length of at least 6 digits.");
    }

}
