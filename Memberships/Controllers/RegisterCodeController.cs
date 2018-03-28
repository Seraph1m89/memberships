using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Memberships.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Infrastructure;

namespace Memberships.Controllers
{
    public class RegisterCodeController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Register(string code)
        {
            if (!Request.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }

            var userId = User.Identity.GetUserId();
            var subscriptionId = await SubscriptionHelpers.GetSubscrptionId(code);
            var registered = await SubscriptionHelpers.TryRegister(subscriptionId, userId);

            if (!registered)
            {
                throw new ApplicationException();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}