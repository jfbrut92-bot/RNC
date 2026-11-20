using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Utac.Rnc2.MVC.Extensions
{
    public static class Helpers
    {
        public static string CheckboxGroup<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> propertySelector, int value) where TProperty : IEnumerable<int>
        {
            var groupName = GetPropertyName(propertySelector);
            var modelValues = propertySelector.Compile().Invoke(htmlHelper.ViewData.Model);

            var svalue = value.ToString();
            var builder = new TagBuilder("input");
            builder.GenerateId(groupName);
            builder.Attributes.Add("type", "checkbox");
            builder.Attributes.Add("name", groupName);
            builder.Attributes.Add("value", svalue);
            var contextValues = HttpContext.Current.Request.Form.GetValues(groupName);
            if ((contextValues != null && contextValues.Contains(svalue)) || (modelValues != null && modelValues.Contains(value)))
            {
                builder.Attributes.Add("checked", null);
            }
            return builder.ToString(TagRenderMode.Normal);
        }

        private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> propertySelector)
        {
            var body = propertySelector.Body.ToString();
            var firstIndex = body.IndexOf('.') + 1;
            return body.Substring(firstIndex);
        }
    }
}
