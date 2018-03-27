using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Memberships.Models;

namespace Memberships.Extensions
{
    public static class ThumbnailExtensions
    {
        private static async Task<List<int>> GetSubscriptionIdAsync(string userId = null, ApplicationDbContext db = null)
        {
            if (userId == null)
            {
                return new List<int>();
            }

            if (db == null)
            {
                db = ApplicationDbContext.Create();
            }

            return await db.UserSubscriptions.Where(us => us.UserId == userId).Select(us => us.SubscriptionId).ToListAsync();
        }

        public static async Task<IEnumerable<ThumbnailModel>> GetProductThumbnailAsync(string userId = null, ApplicationDbContext db = null)
        {
            if (userId == null)
            {
                return new List<ThumbnailModel>();
            }

            if (db == null)
            {
                db = ApplicationDbContext.Create();
            }

            var subscriptionIds = await GetSubscriptionIdAsync(userId, db);
            var thumbs = await (
                from ps in db.SubscriptionProducts
                join p in db.Products on ps.ProductId equals p.Id
                join plt in db.ProductLinkTexts on p.ProductLinkTextId equals plt.Id
                join pt in db.ProductTypes on p.ProductTypeId equals pt.Id
                where subscriptionIds.Contains(ps.SubscriptionId)
                select new ThumbnailModel
                {
                    ProductId = p.Id,
                    SubscriptionId = ps.SubscriptionId,
                    Title = p.Title,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Link = "/ProductContent/Index/" + p.Id,
                    TagText = plt.Title,
                    ContentTag = pt.Title
                }).ToListAsync();

            return thumbs.Distinct().OrderBy(t => t.Title);
        }
    }
}