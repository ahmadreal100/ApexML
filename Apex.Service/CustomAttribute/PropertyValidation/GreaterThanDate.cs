using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apex.Service.CustomAttribute.PropertyValidation
{
    public class GreaterThanDateAttribute : ValidationAttribute, IClientValidatable
    {
        private string OtherProperty { get; }

        public GreaterThanDateAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

            if (otherPropertyInfo != null)
            {
                var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null) ?? "";

                if (value == null)
                    return ValidationResult.Success;

                if (!DateTime.TryParse(value.ToString(), out var _) || !DateTime.TryParse(otherPropertyValue.ToString(), out var _))
                    return new ValidationResult(ErrorMessage);

                return (DateTime?)value > (DateTime?)otherPropertyValue ? ValidationResult.Success : new ValidationResult(ErrorMessage);
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "greaterthandate",
                ErrorMessage = ErrorMessage
            };
            //For pass data as parameter to js like rule in html element.[A.R]
            rule.ValidationParameters.Add("otherproperty", OtherProperty);
            return new List<ModelClientValidationRule> { rule };
        }
    }
}