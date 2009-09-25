using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using CustomMarkupExtension.ValidationRules;

namespace CustomMarkupExtension.MarkupExtensions
{
    public class GreaterThan : ValidationBinding
    {
        public int Number { get; set; }

        public override IEnumerable<ValidationRule> GetValidationRules()
        {
            return new List<ValidationRule> {new GreaterThanRule {Number = Number}};
        }
    }
}