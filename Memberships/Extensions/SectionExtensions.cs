using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls.Expressions;
using Memberships.Models;

namespace Memberships.Extensions
{
    public static class SectionExtensions
    {
        public static async Task<ProductSectionModel> GetProductSectionAsync(int productId, string userId)
        {
            ProductSectionModel model;
            using (var db = ApplicationDbContext.Create())
            {
                var sections = (await (from product in db.Products
                                      join productItem in db.ProductItems on product.Id equals productItem.ProductId
                                      join item in db.Items on productItem.ItemId equals item.Id
                                      join section in db.Sections on item.SectionId equals section.Id
                                      where product.Id == productId
                                      select new ProductSection
                                      {
                                          SectionId = section.Id,
                                          ItemTypeId = item.ItemTypeId,
                                          Title = section.Title
                                      }).ToListAsync()).Distinct().ToArray();

                foreach (var section in sections)
                {
                    section.ProductItemRows =
                        await GetProductItemRowAsync(productId, section.SectionId, section.ItemTypeId, userId, db);
                }

                var ordered = sections.OrderBy(s => s.Title.ToLower().Contains("downloads")).ThenBy(s => s.Title);

                model = new ProductSectionModel
                {
                    ProductSections = ordered,
                    Title = await db.Products.Where(product => product.Id == productId)
                        .Select(product => product.Title)
                        .FirstOrDefaultAsync()
                };
            }

            return model;
        }

        public static async Task<IEnumerable<ProductItemRow>> GetProductItemRowAsync(int productId, int sectionId,
            int itemTypeId, string userId, ApplicationDbContext db)
        {
            db = db ?? ApplicationDbContext.Create();

            var today = DateTime.UtcNow;

            var items = await (from item in db.Items
                               join itemType in db.ItemTypes on item.ItemTypeId equals itemType.Id
                               join productItem in db.ProductItems on item.Id equals productItem.ItemId
                               join subscriptionProduct in db.SubscriptionProducts on productItem.ProductId equals subscriptionProduct
                                   .ProductId
                               join userSubscription in db.UserSubscriptions on subscriptionProduct.SubscriptionId equals
                                   userSubscription.SubscriptionId
                               where item.SectionId == sectionId
                                     && productItem.ProductId == productId
                                     && userSubscription.UserId.Equals(userId)
                               orderby item.PartId
                               select new ProductItemRow
                               {
                                   ItemId = item.Id,
                                   Description = item.Description,
                                   Title = item.Title,
                                   Link = itemType.Title.ToLower() == "download" ? item.Url : "/ProductContent/Content/" + productItem.ProductId + "/" + item.Id,
                                   ImageUrl = item.ImageUrl,
                                   ReleaseDate = DbFunctions.CreateDateTime(userSubscription.StartDate.Value.Year,
                                       userSubscription.StartDate.Value.Month,
                                       userSubscription.StartDate.Value.Day + item.WaitDays,
                                       0, 0, 0),
                                   IsAvailable = DbFunctions.CreateDateTime(today.Year,
                                       today.Month,
                                       today.Day,
                                       0, 0, 0)
                                       >= DbFunctions.CreateDateTime(userSubscription.StartDate.Value.Year,
                                          userSubscription.StartDate.Value.Month,
                                          userSubscription.StartDate.Value.Day + item.WaitDays,
                                          0, 0, 0),
                                   IsDownloadable = itemType.Title.ToLower() == "download"
                               }).ToListAsync();

            return items;
        }

        public static async Task<ContentViewModel> GetContentAsync(int productId, int itemId)
        {
            var db = ApplicationDbContext.Create();
            return await (from item in db.Items
                          //join itemType in db.ItemTypes on item.ItemTypeId equals itemType.Id
                          where item.Id == itemId
                select new ContentViewModel
                {
                    ProductId = productId,
                    Html = item.Html,
                    Title = item.Title,
                    Description = item.Description,
                    VideoUrl = item.Url
                }).FirstOrDefaultAsync();
        }
    }
}