//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Reflection;
//using System.Web.Mvc;

//namespace Apex.Service.CustomAttribute.PropertyValidation
//{
//    public class DepotToDepotRequiredAttribute : ValidationAttribute, IClientValidatable
//    {
//        private readonly string[] _ifSelectValue = { "4", "13" };
//        private readonly string _selectProperty = "IoTypeId";
//        private readonly string _isDeposit = "IsDeposit";

//        public DepotToDepotRequiredAttribute()
//        {

//        }

//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {
//            PropertyInfo selectPropertyInfo = validationContext.ObjectType.GetProperty(_selectProperty);
//            PropertyInfo isDepositInfo = validationContext.ObjectType.GetProperty(_isDeposit);

//            if (selectPropertyInfo != null && isDepositInfo != null)
//            {
//                var selectPropertyStringValue = selectPropertyInfo.GetValue(validationContext.ObjectInstance, null).ToString();
//                var isDeposit = bool.Parse(isDepositInfo.GetValue(validationContext.ObjectInstance, null).ToString());
//                if (_ifSelectValue.Contains(selectPropertyStringValue) && string.IsNullOrWhiteSpace(value.ToString()))
//                {
//                    return new ValidationResult(ErrorMessage);
//                }

//                return ValidationResult.Success;
//            }

//            return null;
//        }

//        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
//        {
//            var rule = new ModelClientValidationRule
//            {
//                ValidationType = "ifselectrequired",
//                ErrorMessage = ErrorMessage
//            };
//            //For pass data as parameter to js like rule in html element.[A.R]
//            rule.ValidationParameters.Add("selectproperty", SelectProperty);
//            rule.ValidationParameters.Add("ifselectvalue", _ifSelectValue);
//            return new List<ModelClientValidationRule> { rule };
//        }
//    }
//}