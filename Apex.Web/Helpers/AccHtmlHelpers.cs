using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Apex.Shared.Helpers;

namespace Apex.Web.Helpers
{
    public static class AccHtmlHelpers
    {

        public static MvcHtmlString PersianDate(this HtmlHelper htmlHelper, string name)
        {
            return htmlHelper.TextBox(name, null);
        }

        public static MvcHtmlString PersianDate(this HtmlHelper htmlHelper, string name, object value, string pattern = null)
        {
            value = ((DateTime)value).ToPersian(pattern);
            return htmlHelper.TextBox(name, value, (string)null);
        }

        public static MvcHtmlString PersianDate(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes, string pattern = null)
        {
            value = ((DateTime)value).ToPersian(pattern);
            return htmlHelper.TextBox(name, value, null, htmlAttributes);
        }


        //---For
        public static MvcHtmlString PersianDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string pattern = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var value = ((DateTime)metadata.Model).ToPersian(pattern);
            return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), value);
        }

        public static MvcHtmlString PersianDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, string pattern)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var value = ((DateTime)metadata.Model).ToPersian(pattern);
            return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), value, format);
        }

        public static MvcHtmlString PersianDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string pattern = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var value = ((DateTime?)metadata.Model).ToPersian(pattern);
            return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), value, htmlAttributes);
        }

        public static MvcHtmlString PersianDateFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes, string pattern = null)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var value = ((DateTime)metadata.Model).ToPersian(pattern);
            return htmlHelper.TextBox(ExpressionHelper.GetExpressionText(expression), value, format, htmlAttributes);
        }
    }
}