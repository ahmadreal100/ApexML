using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Apex.Service.Extensions;
using Apex.Shared.Extensions;
using Apex.Shared.Helpers;

namespace Apex.Web.Models.Binders
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (bindingContext.ModelType.IsNullable())
                if (string.IsNullOrWhiteSpace(valueProviderResult?.AttemptedValue))
                    return null;
            return valueProviderResult == null ? base.BindModel(controllerContext, bindingContext) : Convert.ToDecimal(valueProviderResult.AttemptedValue);
            // of course replace with your custom conversion logic
        }
    }
    public class DoubleModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var nullable = bindingContext.ModelType.IsNullable();
            var bv = nullable ? valueProviderResult?.AttemptedValue : (valueProviderResult?.AttemptedValue ?? "0").IsNeu("0");
            if (bv == null)
                return null;
            return double.TryParse(Regex.Replace(bv, "\\D", ""), out var i) ? i : (nullable ? (double?)null : 0);
        }
    }
    public class LongModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var nullable = bindingContext.ModelType.IsNullable();
            var bv = nullable ? valueProviderResult?.AttemptedValue : (valueProviderResult?.AttemptedValue ?? "0").IsNeu("0");
            if (bv == null)
                return null;
            return long.TryParse(Regex.Replace(bv, "\\D", ""), out var i) ? i : (nullable ? (long?)null : 0);
        }
    }
    public class IntModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var nullable = bindingContext.ModelType.IsNullable();
            var bv = nullable ? valueProviderResult?.AttemptedValue : (valueProviderResult?.AttemptedValue ?? "0").IsNeu("0");
            if (bv == null)
                return null;
            return int.TryParse(Regex.Replace(bv, "\\D", ""), out var i) ? i : (nullable ? (int?)null : 0);
        }
    }
    public class DateTimeModelBinder : DefaultModelBinder
    {

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var val = valueProviderResult?.AttemptedValue ?? "";

            return Regex.IsMatch(val, @"^\/?Date\(\d+\)\/?$") ?
                new DateTime(1970, 1, 2).AddMilliseconds(double.Parse(Regex.Replace(val, @"\D", ""))) :
                val.ToGregorian();
        }
    }
}