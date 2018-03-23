using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Memberships.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetPropertyValue<T>(this T item, string propertyName)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Can't apply extension to null");
            }

            var property = item.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException("Current property does not exist on the object");
            }

            return property.GetValue(item, null).ToString();
        }
    }
}