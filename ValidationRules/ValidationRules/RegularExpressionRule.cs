using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using CommonLibrary.Extensions;

namespace ValidationRules.ValidationRules
{
    public class RegularExpressionRule : ValidationRule
    {
        public string Regex { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Must match regular expression");

            if (Regex == null)
                return new ValidationResult(false, "Regular Expression must be set");

            Match match = System.Text.RegularExpressions.Regex.Match(value.ToString(), Regex);

            return match.Success
                       ? new ValidationResult(true, null)
                       : new ValidationResult(false,
                                              "{0} must be match {1}".FormatString(value.ToString(), Regex));
        }
    }
}