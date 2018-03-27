﻿using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Memberships.Extensions
{
    public static class HttpContextExtensions
    {
        private const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public static string GetUserId(this HttpContextBase context)
        {
            var userId = string.Empty;
            try
            {
                var claims = context.GetOwinContext().Get<ApplicationSignInManager>().AuthenticationManager.User.Claims
                    .FirstOrDefault(claim => claim.Type == NameIdentifier);

                if (claims != default(Claim))
                {
                    userId = claims.Value;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return userId;
        }
    }
}