using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Apex.Service.CustomAttribute.PropertyValidation
{
    public class MinValueAttribute : ValidationAttribute, IClientValidatable
    {
        private double MinValue { get; }

        public MinValueAttribute(double minValue)
        {
            MinValue = minValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = value ?? "0";
            if (double.TryParse(val.ToString(), out double ut))
            {
                if (ut >= MinValue)
                    return ValidationResult.Success;
                return new ValidationResult(ErrorMessage);
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ValidationType = "minvalue",
                ErrorMessage = ErrorMessage
            };
            //For pass data as parameter to js like rule in html element.[A.R]
            rule.ValidationParameters.Add("mv", MinValue);
            return new List<ModelClientValidationRule> { rule };
        }
    }
}