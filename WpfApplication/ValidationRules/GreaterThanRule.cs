using System;
using System.Globalization;
using System.Windows.Controls;
using WpfApplication.Framework;

namespace WpfApplication.ValidationRules
{
    public class GreaterThanRule : ValidationRule
    {
        public int? Number { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Must be a number");

            if (Number == null)
                return new ValidationResult(false, "Number must be set");

            int v;
            if (!Int32.TryParse(value.ToString(), out v))
                return new ValidationResult(false, "Must be a number");

            return v > Number
                       ? new ValidationResult(true, null)
                       : new ValidationResult(false, "{0} must be greater than {1}".FormatString(v, Number));
        }
    }
}