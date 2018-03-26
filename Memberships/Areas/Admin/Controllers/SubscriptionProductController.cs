using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
    public class SubscriptionProductController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/SubscriptionProduct
        public async Task<ActionResult> Index()
        {
            var subscriptionProducts = await db.SubscriptionProducts.ToListAsync();
            var dbSubscriptions = await db.Subscriptions.ToListAsync();
            var dbProducts = await db.Products.ToListAsync();

            var modelList = Mapper.Map<IEnumerable<SubscriptionProductViewModel>>(subscriptionProducts, options => options.AfterMap(
                (source, target) =>
                {
                    if (target is IEnumerable<SubscriptionProductViewModel> models)
                    {
                        foreach (var model in models)
                        {
                            var dbItem = dbSubscriptions.FirstOrDefault(item => item.Id == model.SubscriptionId);
                            var dbProduct = dbProducts.FirstOrDefault(prod => prod.Id == model.ProductId);
                            model.SubscriptionTitle = dbItem?.Title;
                            model.ProductTitle = dbProduct?.Title;
                        }
                    }
                })
            );

            return View(modelList);
        }

        // GET: Admin/SubscriptionProduct/Details/5
        public async Task<ActionResult> Details(int? subscriptionId, int? productId)
        {
            if (subscriptionId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionProduct subscriptionProduct = await GetSubscriptionProduct(subscriptionId, productId);
            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }

            var subscription = await db.Subscriptions.FirstOrDefaultAsync(dbItem => dbItem.Id == subscriptionProduct.SubscriptionId);
            var product = await db.Products.FirstOrDefaultAsync(dbProduct => dbProduct.Id == subscriptionProduct.ProductId);
            var model = Mapper.Map<SubscriptionProductViewModel>(subscriptionProduct, options => options.AfterMap((source, target) =>
            {
                if (target is SubscriptionProductViewModel piModel)
                {
                    piModel.SubscriptionTitle = subscription?.Title;
                    piModel.ProductTitle = product?.Title;
                }
            }));

            return View(model);
        }

        // GET: Admin/SubscriptionProduct/Create
        public async Task<ActionResult> Create()
        {
            var model = new SubscriptionProductViewModel()
            {
                Subscriptions = await db.Subscriptions.ToListAsync(),
                Products = await db.Products.ToListAsync()
            };

            return View(model);
        }

        // POST: Admin/SubscriptionProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubscriptionId,ProductId")] SubscriptionProduct subscriptionProduct)
        {
            if (ModelState.IsValid)
            {
                db.SubscriptionProducts.Add(subscriptionProduct);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var model = await GetSubscriptionProductModel(subscriptionProduct);

            return View(model);
        }



        // GET: Admin/SubscriptionProduct/Edit/5
        public async Task<ActionResult> Edit(int? subscriptionId, int? productId)
        {
            if (subscriptionId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionProduct subscriptionProduct = await GetSubscriptionProduct(subscriptionId, productId);
            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }

            var model = await GetSubscriptionProductModel(subscriptionProduct);
            return View(model);
        }

        // POST: Admin/SubscriptionProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SubscriptionProduct subscriptionProduct)
        {
            if (ModelState.IsValid)
            {
                await Change(subscriptionProduct);
                return RedirectToAction("Index");
            }

            var model = await GetSubscriptionProductModel(subscriptionProduct);
            return View(model);
        }

        // GET: Admin/SubscriptionProduct/Delete/5
        public async Task<ActionResult> Delete(int? subscriptionId, int? productId)
        {
            if (subscriptionId == null || productId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubscriptionProduct subscriptionProduct = await GetSubscriptionProduct(subscriptionId, productId);
            if (subscriptionProduct == null)
            {
                return HttpNotFound();
            }

            var model = await GetSubscriptionProductModel(subscriptionProduct);
            return View(model);
        }

        // POST: Admin/SubscriptionProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? subscriptionId, int? productId)
        {
            SubscriptionProduct subscriptionProduct = await GetSubscriptionProduct(subscriptionId, productId);
            db.SubscriptionProducts.Remove(subscriptionProduct);
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

        // Definetly should go to DAL
        private async Task Change(SubscriptionProduct productItem)
        {
            var currentProductDb = await db.SubscriptionProducts.FirstOrDefaultAsync(pi =>
                pi.SubscriptionId == productItem.OldSubscriptionId && pi.ProductId == productItem.OldProductId);

            var isNewItemAlreadyExist = await db.SubscriptionProducts.AnyAsync(pi =>
                pi.SubscriptionId == productItem.SubscriptionId && pi.ProductId == productItem.ProductId);

            if (currentProductDb != null && !isNewItemAlreadyExist)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    db.SubscriptionProducts.Remove(currentProductDb);
                    db.SubscriptionProducts.Add(productItem);
                    await db.SaveChangesAsync();
                    transaction.Complete();
                }
            }
        }

        private async Task<SubscriptionProduct> GetSubscriptionProduct(int? subscriptionId, int? productId)
        {
            if (!subscriptionId.HasValue || !productId.HasValue)
            {
                return null;
            }

            return await db.SubscriptionProducts.FirstOrDefaultAsync(pi => pi.SubscriptionId == subscriptionId.Value && pi.ProductId == productId.Value);
        }

        private async Task<SubscriptionProductViewModel> GetSubscriptionProductModel(SubscriptionProduct subscriptionProduct)
        {
            var model = Mapper.Map<SubscriptionProductViewModel>(subscriptionProduct);
            model.Subscriptions = await db.Subscriptions.ToListAsync();
            model.Products = await db.Products.ToListAsync();
            return model;
        }
    }
}
