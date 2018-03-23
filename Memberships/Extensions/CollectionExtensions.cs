using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Memberships.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this ICollection<T> collection, int selectedValue)
        {
            foreach (var item in collection)
            {
                var id = item.GetPropertyValue("Id");

                yield return new SelectListItem
                {
                    Text = item.GetPropertyValue("Title"),
                    Value = id,
                    Selected = id.Equals(selectedValue.ToString())
                };
            }
        }
    }
}