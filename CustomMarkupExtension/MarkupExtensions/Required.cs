using System.Collections.Generic;
using System.Windows.Controls;
using CustomMarkupExtension.ValidationRules;

namespace CustomMarkupExtension.MarkupExtensions
{
    public class Required : ValidationBinding
    {
        public override IEnumerable<ValidationRule> GetValidationRules()
        {
            return new List<ValidationRule> {new RequiredRule()};
        }
    }
}