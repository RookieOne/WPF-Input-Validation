using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CommonLibrary.Extensions;

namespace CustomMarkupExtension.MarkupExtensions
{
    public abstract class ValidationBinding : BindingDecoratorBase
    {
        protected ValidationBinding()
        {
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }

        public abstract IEnumerable<ValidationRule> GetValidationRules();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            GetValidationRules()
                        .ForEach(r => ValidationRules.Add(r));
            
            return base.ProvideValue(serviceProvider);
        }
    }
}