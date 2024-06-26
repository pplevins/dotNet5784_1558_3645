﻿using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PL.validation;
public class StringValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is string name)
        {
            // Regular expression pattern for validating strings
            string pattern = @"^[a-zA-Z\s]{3,}$";

            if (Regex.IsMatch(name, pattern))
                return ValidationResult.ValidResult;
        }

        return new ValidationResult(false, "Field must contain only letters and be at least 3 characters long.");
    }
}