using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apex.Service.CustomAttribute.PropertyValidation
{
    public class MaxValueAttribute : ValidationAttribute, IClientValidatable
    {
        private double MaxValue { get; }

        public MaxValueAttribute(double maxValue)
        {
            MaxValue = maxValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = value ?? "0";
            if (double.TryParse(val.ToString(), out var ut))
            {
                if (ut <= MaxValue)
                    return ValidationResult.Success;
                return new ValidationResult(ErrorMessage);
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "maxvalue",
                ErrorMessage = ErrorMessage
            };
            //For pass data as parameter to js like rule in html element.[A.R]
            rule.ValidationParameters.Add("mv", MaxValue);
            return new List<ModelClientValidationRule> { rule };
        }
    }
}