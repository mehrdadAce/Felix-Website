using System;
using System.Linq.Expressions;
using Umbraco.Core.Models;
using Umbraco.Web.PublishedContentModels;

namespace FelixWebsite.Core.Helpers
{
    public static class ContentHelper
    {
        public static void SetTypedValue<T, TResult>(this IContent content, Expression<Func<T, TResult>> propertyExpression, TResult value) where T : IPublishedContent
        {
            var expression = (MemberExpression)propertyExpression.Body;
            content.SetValue(expression.Member.Name, value);
        }

        public static TResult GetTypedValue<T, TResult>(this IContent content, Expression<Func<T, TResult>> propertyExpression) where T : IPublishedContent
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var x = expression.Member.Name;
            var y = content.GetValue<TResult>(x);
            return content.GetValue<TResult>(expression.Member.Name);
        }

        public static bool IsHomePage(this IPublishedContent page)
        {
            if (page is GroupHome || page is BusinessBrand || page is BusinessCombined)
                return true;
            return false;
        }
    }
}