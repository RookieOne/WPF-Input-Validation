using System;
using System.Globalization;
using System.Windows.Controls;

namespace WpfApplication.ValidationRules
{
    public class RequiredRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Is Required");

            if (String.IsNullOrEmpty(value.ToString()))
                return new ValidationResult(false, "Is Required");

            return new ValidationResult(true, null);
        }
    }
}