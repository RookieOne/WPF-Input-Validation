using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration;
using Bennedik.Validation.Integration.WPF.Properties;

namespace Bennedik.Validation.Integration.WPF
{
    /// <summary>
    /// The EnterpriseValidationRule integrates WPF with the Validation Application Block (VAB) of Enterprise Library 3.0.
    /// It is similar to the VAB ASP.NET integration's PropertyProxyValidator but implements a 
    /// WPF ValidationRule instead of an ASP.NET BaseValidator.
    /// An ErrorProvider can be used to conveniently initialize EnterpriseValidationRules.
    /// </summary>
    /// <remarks>
    /// (c) 2007 Martin Bennedik, see BSD license in the license.txt file
    /// http://www.bennedik.de
    /// </remarks>
    public class EnterpriseValidationRule : ValidationRule, IValidationIntegrationProxy
    {
        protected object value;

        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            this.value = value;

            Validator validator = new ValidationIntegrationHelper(this).GetValidator();

            if (validator != null)
            {
                ValidationResults validationResults = validator.Validate(this);

                string errorMessage = FormatErrorMessage(validationResults);
                return new System.Windows.Controls.ValidationResult(validationResults.IsValid, errorMessage);
            }
            else
            {
                return new System.Windows.Controls.ValidationResult(true, null);
            }
        }

        internal static string FormatErrorMessage(ValidationResults results)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (!results.IsValid)
            {
                bool needsComma = false;
                foreach (Microsoft.Practices.EnterpriseLibrary.Validation.ValidationResult validationResult in results)
                {
                    if (needsComma)
                        stringBuilder.Append(", ");
                    else
                        needsComma = true;

                    stringBuilder.Append(validationResult.Message);
                }
            }

            return stringBuilder.ToString();
        }

        internal bool GetValue(out object value, out string valueAccessFailureMessage)
        {
            ValidationIntegrationHelper helper = new ValidationIntegrationHelper(this);

            bool result;
            try
            {
                result = helper.GetValue(out value, out valueAccessFailureMessage);
            }
            catch
            {
                result = false;
                value = null;
                valueAccessFailureMessage = String.Empty;
            }
            
            return result;
        }

        private string sourceTypeName;
        /// <summary>
        /// Gets or sets the name of the type to use a source for validation specifications.
        /// </summary>
        public string SourceTypeName
        {
            get { return sourceTypeName; }
            set { sourceTypeName = value; }
        }

        private string propertyName;
        /// <summary>
        /// Gets or sets the name of the property to use as soource for validation specifications.
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        private string rulesetName;
        /// <summary>
        /// Gets or sets the name of the ruleset to use when retrieving validation specifications.
        /// </summary>
        [DefaultValue("")]
        public string RulesetName
        {
            get { return rulesetName != null ? rulesetName : string.Empty; }
            set { rulesetName = value; }
        }

        private ValidationSpecificationSource specificationSource = ValidationSpecificationSource.Both;
        /// <summary>
        /// Gets or sets the <see cref="ValidationSpecificationSource"/> indicating where to get validation specifications from.
        /// </summary>
        [DefaultValue(ValidationSpecificationSource.Both)]
        public ValidationSpecificationSource SpecificationSource
        {
            get { return specificationSource; }
            set { specificationSource = value; }
        }

        private IValueConverter converter;

        public IValueConverter Converter
        {
            get { return converter; }
            set { converter = value; }
        }

        private CultureInfo converterCulture;

        public CultureInfo ConverterCulture
        {
            get { return converterCulture; }
            set { converterCulture = value; }
        }

        private object converterParameter;

        public object ConverterParameter
        {
            get { return converterParameter; }
            set { converterParameter = value; }
        }

        #region IValidationIntegrationProxy Members

        object IValidationIntegrationProxy.GetRawValue()
        {
            return value;
        }

        MemberValueAccessBuilder IValidationIntegrationProxy.GetMemberValueAccessBuilder()
        {
            return new PropertyMappedValidatorValueAccessBuilder();
        }

        void IValidationIntegrationProxy.PerformCustomValueConversion(ValueConvertEventArgs e)
        {
            if (this.Converter != null)
            {
                try
                {
                    e.ConvertedValue = Converter.ConvertBack(e.ValueToConvert, e.TargetType, ConverterParameter, ConverterCulture);
                }
                catch (Exception x)
                {
                    e.ConversionErrorMessage = x.Message;
                }
            }
        }

        bool IValidationIntegrationProxy.ProvidesCustomValueConversion
        {
            get { return this.Converter != null; }
        }

        string IValidationIntegrationProxy.Ruleset
        {
            get { return this.RulesetName; }
        }

        ValidationSpecificationSource IValidationIntegrationProxy.SpecificationSource
        {
            get { return this.SpecificationSource; }
        }

        string IValidationIntegrationProxy.ValidatedPropertyName
        {
            get { return this.PropertyName; }
        }

        Type IValidationIntegrationProxy.ValidatedType
        {
            get
            {
                if (string.IsNullOrEmpty(this.sourceTypeName))
                {
                    throw new InvalidOperationException(Resources.ExceptionNullSourceTypeName);
                }
                Type validatedType = Type.GetType(this.SourceTypeName, false, false);
                if (validatedType == null)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.CurrentUICulture,
                            Resources.ExceptionInvalidSourceTypeName,
                            this.sourceTypeName));
                }

                return validatedType;
            }
        }

        #endregion
    }
}
