using System;
using System.Windows.Markup;

namespace CustomMarkupExtension.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof (object))]
    public class MyCustomMarkupExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}