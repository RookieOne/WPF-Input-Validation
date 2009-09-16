using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Bennedik.Validation.Integration.WPF.Properties;

namespace Bennedik.Validation.Integration.WPF
{
    /// <summary>
    /// This helper class was copied from the ASP.NET VAB integration.
    /// </summary>
    internal class PropertyMappedValidatorValueAccessBuilder : MemberValueAccessBuilder
    {
        protected override ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo)
        {
            throw new NotSupportedException();
        }

        protected override ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo)
        {
            throw new NotSupportedException();
        }

        protected override ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo)
        {
            return new PropertyMappedValidatorValueAccess(propertyInfo.Name);
        }
    }

    /// <summary>
    /// This helper class was copied from the ASP.NET VAB integration.
    /// </summary>
    internal class PropertyMappedValidatorValueAccess : ValueAccess
    {
        private string propertyName;

        public PropertyMappedValidatorValueAccess(string propertyName)
        {
            this.propertyName = propertyName;
        }

        public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
        {
            value = null;
            valueAccessFailureMessage = null;

            EnterpriseValidationRule validator = source as EnterpriseValidationRule;

            if (this.propertyName.Equals(validator.PropertyName))
            {
                return validator.GetValue(out value, out valueAccessFailureMessage);
            }

            valueAccessFailureMessage = string.Format(CultureInfo.CurrentUICulture,
                Resources.ErrorNonMappedProperty,
                this.propertyName);
            return false;
        }

        public override string Key
        {
            get { return this.propertyName; }
        }
    }
}
