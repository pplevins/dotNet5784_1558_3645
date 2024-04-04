using System.Globalization;
using System.Windows.Controls;

namespace PL.validation;
public class PasswordLengthValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value?.ToString().Length >= 6)
        {
            return ValidationResult.ValidResult;
        }
        else
        {
            return new ValidationResult(false, "Password must be at least 6 characters long.");
        }
    }
}