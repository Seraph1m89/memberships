using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;

namespace Memberships.Areas.Admin.Extensions
{
    public static class ConvertionExtensions
    {
        public static async Task<IEnumerable<ProductModel>> Convert(this IEnumerable<Product> products,
            ApplicationDbContext context)
        {
            if (!products.Any())
            {
                return new List<ProductModel>();
            }

            var texts = await context.ProductLinkTexts.ToArrayAsync();
            var types = await context.ProductTypes.ToListAsync();

            return products.Select(p => new ProductModel
            {
                ProductTypes = types,
                ProductLinkTexts = texts,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Id = p.Id,
                Title = p.Title,
                ProductTypeId = p.ProductTypeId,
                ProductLinkTextId = p.ProductLinkTextId
            });
        }
    }
}