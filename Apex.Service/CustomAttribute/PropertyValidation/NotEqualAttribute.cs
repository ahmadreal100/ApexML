using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace Apex.Service.CustomAttribute.PropertyValidation
{
    public class NotEqualAttribute : ValidationAttribute, IClientValidatable
    {
        private string OtherProperty { get; }

        public NotEqualAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

            if (otherPropertyInfo != null)
            {
                var otherPropertyStringValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null).ToString();

                if (value.ToString() == otherPropertyStringValue)
                {
                    return new ValidationResult(ErrorMessage);
                }

                return ValidationResult.Success;
            }

            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "notequal",
                ErrorMessage = ErrorMessage
            };
            //For pass data as parameter to js like rule in html element.[A.R]
            rule.ValidationParameters.Add("otherproperty", OtherProperty);
            return new List<ModelClientValidationRule> { rule };
        }
    }
}