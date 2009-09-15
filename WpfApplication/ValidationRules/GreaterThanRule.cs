using System;
using System.Globalization;
using System.Windows.Controls;
using WpfApplication.Framework;

namespace WpfApplication.ValidationRules
{
    public class GreaterThanRule : ValidationRule
    {
        private readonly int _number;

        public GreaterThanRule(int number)
        {
            _number = number;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Must be a number");

            int v;
            if (!Int32.TryParse(value.ToString(), out v))
                return new ValidationResult(false, "Must be a number");

            return v > _number
                       ? new ValidationResult(true, null)
                       : new ValidationResult(false, "{0} must be greater than {1}".FormatString(v, _number));
        }
    }
}