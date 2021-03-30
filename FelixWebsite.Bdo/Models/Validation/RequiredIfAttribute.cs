using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FelixWebsite.Bdo.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RequiredIfAttribute : ValidationAttribute, IClientValidatable
    {
        public string OtherProperty { get; private set; }

        public RequiredIfAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var conditionalProperty = validationContext.ObjectInstance.GetType()
                         .GetProperty(OtherProperty);

            if (conditionalProperty == null || conditionalProperty.PropertyType != typeof(bool))
                return new ValidationResult(FormatErrorMessage(ErrorMessageResourceName));

            var conditionalPropertyValue = (bool)conditionalProperty.GetValue(validationContext.ObjectInstance, null);

            if (conditionalPropertyValue && string.IsNullOrEmpty((string)value))
                return new ValidationResult(FormatErrorMessage(ErrorMessageResourceName));

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = nameof(RequiredIfAttribute).ToLower().Replace("attribute", string.Empty)
            };

            clientValidationRule.ValidationParameters.Add(nameof(OtherProperty).ToLower(), OtherProperty);

            return new[] { clientValidationRule };
        }
    }
}