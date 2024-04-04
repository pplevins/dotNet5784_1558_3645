using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PL.validation;
public class EmailValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string email)
        {
            // Regular expression pattern for email validation
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (Regex.IsMatch(email, pattern))
                return ValidationResult.ValidResult;
        }

        return new ValidationResult(false, "Invalid email address format.");
    }
}
