using System.Collections.Generic;
using System.Windows.Controls;
using CommonLibrary.Consts;
using CustomMarkupExtension.ValidationRules;

namespace CustomMarkupExtension.MarkupExtensions
{
    public class Email : ValidationBinding
    {        
        public override IEnumerable<ValidationRule> GetValidationRules()
        {
            return new List<ValidationRule> {new RegularExpressionRule {Regex = RegularExpressions.Email}};
        }
    }
}