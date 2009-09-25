using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using Bennedik.Validation.Integration.WPF.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration;
using ValidationResult=System.Windows.Controls.ValidationResult;

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
        private IValueConverter converter;
        private CultureInfo converterCulture;
        private object converterParameter;
        private string propertyName;
        private string rulesetName;
        private string sourceTypeName;
        private ValidationSpecificationSource specificationSource = ValidationSpecificationSource.Both;
        protected object value;

        /// <summary>
        /// Gets or sets the name of the type to use a source for validation specifications.
        /// </summary>
        public string SourceTypeName
        {
            get { return sourceTypeName; }
            set { sourceTypeName = value; }
        }

        /// <summary>
        /// Gets or sets the name of the property to use as soource for validation specifications.
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        /// <summary>
        /// Gets or sets the name of the ruleset to use when retrieving validation specifications.
        /// </summary>
        [DefaultValue("")]
        public string RulesetName
        {
            get { return rulesetName != null ? rulesetName : string.Empty; }
            set { rulesetName = value; }
        }

        public IValueConverter Converter
        {
            get { return converter; }
            set { converter = value; }
        }

        public CultureInfo ConverterCulture
        {
            get { return converterCulture; }
            set { converterCulture = value; }
        }

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
            if (Converter != null)
            {
                try
                {
                    e.ConvertedValue = Converter.ConvertBack(e.ValueToConvert, e.TargetType, ConverterParameter,
                                                             ConverterCulture);
                }
                catch (Exception x)
                {
                    e.ConversionErrorMessage = x.Message;
                }
            }
        }

        bool IValidationIntegrationProxy.ProvidesCustomValueConversion
        {
            get { return Converter != null; }
        }

        string IValidationIntegrationProxy.Ruleset
        {
            get { return RulesetName; }
        }

        ValidationSpecificationSource IValidationIntegrationProxy.SpecificationSource
        {
            get { return SpecificationSource; }
        }

        string IValidationIntegrationProxy.ValidatedPropertyName
        {
            get { return PropertyName; }
        }

        Type IValidationIntegrationProxy.ValidatedType
        {
            get
            {
                if (string.IsNullOrEmpty(sourceTypeName))
                {
                    throw new InvalidOperationException(Resources.ExceptionNullSourceTypeName);
                }
                Type validatedType = Type.GetType(SourceTypeName, false, false);
                if (validatedType == null)
                {
                    throw new InvalidOperationException(
                        string.Format(CultureInfo.CurrentUICulture,
                                      Resources.ExceptionInvalidSourceTypeName,
                                      sourceTypeName));
                }

                return validatedType;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets the <see cref="ValidationSpecificationSource"/> indicating where to get validation specifications from.
        /// </summary>
        [DefaultValue(ValidationSpecificationSource.Both)]
        public ValidationSpecificationSource SpecificationSource
        {
            get { return specificationSource; }
            set { specificationSource = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            this.value = value;

            Validator validator = new ValidationIntegrationHelper(this).GetValidator();

            if (validator != null)
            {
                ValidationResults validationResults = validator.Validate(this);

                string errorMessage = FormatErrorMessage(validationResults);
                return new ValidationResult(validationResults.IsValid, errorMessage);
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }

        internal static string FormatErrorMessage(ValidationResults results)
        {
            var stringBuilder = new StringBuilder();

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
            var helper = new ValidationIntegrationHelper(this);

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
    }
}