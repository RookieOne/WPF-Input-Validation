using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Bennedik.Validation.Integration.WPF
{
    /// <summary>
    /// This ErrorProvider is adapted to the Validation Application Block from Paul Stovell's
    /// CodeProject article at http://www.codeproject.com/WPF/wpfvalidation.asp.
    /// The ErrorProvider is a Decorator. It adds an EnterpriseValidationRule to all contained Bindings.
    /// </summary>
    /// <remarks>
    /// (c) 2007, 2008 Martin Bennedik, see BSD license in the license.txt file
    /// http://www.bennedik.de
    /// </remarks>
    public class ErrorProvider : Decorator
    {
        private delegate void FoundBindingCallbackDelegate(FrameworkElement element, Binding binding, DependencyProperty dp);
        private FrameworkElement _firstInvalidElement;
        private List<string> errorMessages;

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

        public ErrorProvider()
        {
            this.Loaded += new RoutedEventHandler(ErrorProvider_Loaded);
        }

        private void ErrorProvider_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        public void Init()
        {
            FindBindingsRecursively(Parent, new FoundBindingCallbackDelegate(AddEnterpriseValidationRule));
            Validate();
        }

        private void AddEnterpriseValidationRule(FrameworkElement element, Binding binding, DependencyProperty dp)
        {
            if (element.DataContext == null)
                return;

            foreach (ValidationRule validationRule in binding.ValidationRules)
            {
                if (validationRule is EnterpriseValidationRule)
                    return; //enterprise validation rule already exists for this binding, no need to add again
            }

            EnterpriseValidationRule rule = new EnterpriseValidationRule();
            Type propertyType = element.DataContext.GetType();
            rule.SourceTypeName = propertyType.AssemblyQualifiedName;
            rule.PropertyName = binding.Path.Path;

            if (rule.PropertyName.Length == 0)
                return; //don't validate XML bindings

            PropertyInfo propertyInfo = propertyType.GetProperty(rule.PropertyName);

            //validate property paths, e.g. Address.Street
            int dot = rule.PropertyName.IndexOf('.');
            while (dot >= 0)
            {
                string propertyName = rule.PropertyName.Substring(0, dot);
                propertyInfo = propertyType.GetProperty(propertyName);

                if (propertyInfo == null)
                    return;

                propertyType = propertyInfo.PropertyType;
                rule.SourceTypeName = propertyType.AssemblyQualifiedName;
                rule.PropertyName = rule.PropertyName.Substring(dot + 1);
                dot = rule.PropertyName.IndexOf('.');
            }

            if (propertyInfo == null)
                return;

            rule.RulesetName = RulesetName;
            rule.SpecificationSource = SpecificationSource;

            rule.Converter = binding.Converter;
            rule.ConverterCulture = binding.ConverterCulture;
            rule.ConverterParameter = binding.ConverterParameter;

            binding.ValidationRules.Add(rule);
        }

        /// <summary>
        /// Validates all properties on the current data source.
        /// </summary>
        /// <returns>True if there are no errors displayed, otherwise false.</returns>
        /// <remarks>
        /// Note that only errors on properties that are displayed are included. Other errors, such as errors for properties that are not displayed, 
        /// will not be validated by this method.
        /// </remarks>
        public bool Validate()
        {
            bool isValid = true;
            _firstInvalidElement = null;
            errorMessages = new List<string>();

            FindBindingsRecursively(Parent,
                delegate(FrameworkElement element, Binding binding, DependencyProperty dp)
                {
                    foreach (ValidationRule rule in binding.ValidationRules)
                    {
                        System.Windows.Controls.ValidationResult valid = rule.Validate(element.GetValue(dp), CultureInfo.CurrentUICulture);
                        if (!valid.IsValid)
                        {
                            if (isValid)
                            {
                                isValid = false;
                                _firstInvalidElement = element;
                            }

                            BindingExpression expression = element.GetBindingExpression(dp);
                            ValidationError error = new ValidationError(rule, expression, valid.ErrorContent, null);
                            System.Windows.Controls.Validation.MarkInvalid(expression, error);

                            string errorMessage = valid.ErrorContent.ToString();
                            if (!errorMessages.Contains(errorMessage))
                                errorMessages.Add(errorMessage);
                        }
                    }
                });

            return isValid;
        }

        /// <summary>
        /// Returns the first element that this error provider has labelled as invalid. This method 
        /// is useful to set the users focus on the first visible error field on a page.
        /// </summary>
        /// <returns></returns>
        public FrameworkElement GetFirstInvalidElement()
        {
            return _firstInvalidElement;
        }

        public List<string> ErrorMessages
        {
            get { return errorMessages; }
        }

        /// <summary>
        /// Recursively goes through the control tree, looking for bindings on the current data context.
        /// </summary>
        /// <param name="element">The root element to start searching at.</param>
        /// <param name="callbackDelegate">A delegate called when a binding if found.</param>
        private void FindBindingsRecursively(DependencyObject element, FoundBindingCallbackDelegate callbackDelegate)
        {
            // See if we should display the errors on this element
            MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.FlattenHierarchy);

            foreach (MemberInfo member in members)
            {
                DependencyProperty dp = null;

                // Check to see if the field or property we were given is a dependency property
                if (member.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    if (typeof(DependencyProperty).IsAssignableFrom(field.FieldType))
                    {
                        dp = (DependencyProperty)field.GetValue(element);
                    }
                }
                else if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo prop = (PropertyInfo)member;
                    if (typeof(DependencyProperty).IsAssignableFrom(prop.PropertyType))
                    {
                        dp = (DependencyProperty)prop.GetValue(element, null);
                    }
                }

                if (dp != null)
                {
                    // Awesome, we have a dependency property. does it have a binding? If yes, is it bound to the property we're interested in?
                    Binding bb = BindingOperations.GetBinding(element, dp);
                    if (bb != null)
                    {
                        // This element has a DependencyProperty that we know of that is bound to the property we're interested in. 
                        // Now we just tell the callback and the caller will handle it.
                        if (element is FrameworkElement)
                        {
                            callbackDelegate((FrameworkElement)element, bb, dp);
                        }
                    }
                }
            }

            // Now, recurse through any child elements
            if (element is FrameworkElement || element is FrameworkContentElement)
            {
                foreach (object childElement in LogicalTreeHelper.GetChildren(element))
                {
                    if (childElement is DependencyObject)
                    {
                        FindBindingsRecursively((DependencyObject)childElement, callbackDelegate);
                    }
                }
            }
        }
    }
}
