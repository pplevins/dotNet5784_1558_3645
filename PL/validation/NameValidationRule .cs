using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PL.validation;
public class NameValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string name)
        {
            // Regular expression pattern for validating names
            string pattern = @"^[a-zA-Z\s]{3,}$";

            if (Regex.IsMatch(name, pattern))
                return ValidationResult.ValidResult;
        }

        return new ValidationResult(false, "Name must contain only letters and be at least 3 characters long.");
    }
}