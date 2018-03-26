using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;

namespace Memberships.Areas.Admin.Controllers
{
    public class ProductItemController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ProductItem
        public async Task<ActionResult> Index()
        {
            var productItems = await db.ProductItems.ToListAsync();
            var dbItems = await db.Items.ToListAsync();
            var dbProducts = await db.Products.ToListAsync();

            var modelList = Mapper.Map<IEnumerable<ProductItemViewModel>>(productItems, options => options.AfterMap(
                (source, target) =>
                {
                    if (target is IEnumerable<ProductItemViewModel> models)
                    {
                        foreach (var model in models)
                        {
                            var dbItem = dbItems.FirstOrDefault(item => item.Id == model.ItemId);
                            var dbProduct = dbProducts.FirstOrDefault(prod => prod.Id == model.ProductId);
                            model.ItemTitle = dbItem?.Title;
                            model.ProductTitle = dbProduct?.Title;
                        }
                    }
                })
            );

            return View(modelList);
        }

        // GET: Admin/ProductItem/Details/5
        public async Task<ActionResult> Details(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }

            var item = await db.Items.FirstOrDefaultAsync(dbItem => dbItem.Id == productItem.ItemId);
            var product = await db.Products.FirstOrDefaultAsync(dbProduct => dbProduct.Id == productItem.ProductId);
            var model = Mapper.Map<ProductItemViewModel>(productItem, options => options.AfterMap((source, target) =>
            {
                if (target is ProductItemViewModel piModel)
                {
                    piModel.ItemTitle = item?.Title;
                    piModel.ProductTitle = product?.Title;
                }
            }));

            return View(model);
        }

        // GET: Admin/ProductItem/Create
        public async Task<ActionResult> Create()
        {
            var model = new ProductItemViewModel
            {
                Items = await db.Items.ToListAsync(),
                Products = await db.Products.ToListAsync()
            };

            return View(model);
        }

        // POST: Admin/ProductItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,ItemId")] ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                db.ProductItems.Add(productItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var model = Mapper.Map<ProductItemViewModel>(productItem);
            model.Items = await db.Items.ToListAsync();
            model.Products = await db.Products.ToListAsync();

            return View(model);
        }

        // GET: Admin/ProductItem/Edit/5
        public async Task<ActionResult> Edit(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }
            var model = Mapper.Map<ProductItemViewModel>(productItem);
            model.Items = await db.Items.ToListAsync();
            model.Products = await db.Products.ToListAsync();

            return View(model);
        }

        // POST: Admin/ProductItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductItem productItem)
        {
            if (ModelState.IsValid)
            {
                await Change(productItem);
                return RedirectToAction("Index");
            }

            var model = Mapper.Map<ProductItemViewModel>(productItem);
            model.Items = await db.Items.ToListAsync();
            model.Products = await db.Products.ToListAsync();
            return View(model);
        }

        // Definetly should go to DAL
        private async Task Change(ProductItem productItem)
        {
            var currentProductDb = await db.ProductItems.FirstOrDefaultAsync(pi =>
                pi.ItemId == productItem.OldItemId && pi.ProductId == productItem.OldProductId);

            var isNewItemAlreadyExist = await db.ProductItems.AnyAsync(pi =>
                pi.ItemId == productItem.ItemId && pi.ProductId == productItem.ProductId);

            if (currentProductDb != null && !isNewItemAlreadyExist)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    db.ProductItems.Remove(currentProductDb);
                    db.ProductItems.Add(productItem);
                    await db.SaveChangesAsync();
                    transaction.Complete();
                }
            }
        }

        // GET: Admin/ProductItem/Delete/5
        public async Task<ActionResult> Delete(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }

            var item = await db.Items.FirstOrDefaultAsync(dbItem => dbItem.Id == productItem.ItemId);
            var product = await db.Products.FirstOrDefaultAsync(dbProduct => dbProduct.Id == productItem.ProductId);
            var model = Mapper.Map<ProductItemViewModel>(productItem, options => options.AfterMap((source, target) =>
            {
                if (target is ProductItemViewModel piModel)
                {
                    piModel.ItemTitle = item?.Title;
                    piModel.ProductTitle = product?.Title;
                }
            }));

            return View(model);
        }

        // POST: Admin/ProductItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? itemId, int? productId)
        {
            if (itemId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productItem = await GetProductItem(itemId, productId);
            if (productItem == null)
            {
                return HttpNotFound();
            }

            db.ProductItems.Remove(productItem);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<ProductItem> GetProductItem(int? itemId, int? productId)
        {
            if (!itemId.HasValue || !productId.HasValue)
            {
                return null;
            }

            return await db.ProductItems.FirstOrDefaultAsync(pi => pi.ItemId == itemId.Value && pi.ProductId == productId.Value);
        }
    }
}
