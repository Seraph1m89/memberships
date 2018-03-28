using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Memberships.Entities;
using Memberships.Models;

namespace Memberships.Helpers
{
    public static class SubscriptionHelpers
    {
        public static async Task<int?> GetSubscrptionId(string registrationCode)
        {
            if (string.IsNullOrWhiteSpace(registrationCode))
            {
                return null;
            }

            int subscriptionId;
            using (var db = ApplicationDbContext.Create())
            {
                try
                {
                    subscriptionId = await db.Subscriptions
                        .Where(subscription => subscription.RegistrationCode == registrationCode).Select(subscription => subscription.Id).FirstAsync();
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine("No subscriptions with given code found");
                    return null;
                }
            }

            return subscriptionId;
        }

        public static async Task<bool> TryRegister(int? subscriptionId, string userId)
        {
            if (subscriptionId == null || string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }

            using (var db = ApplicationDbContext.Create())
            {
                var subscriptionExist = await db.UserSubscriptions.AnyAsync(subscription =>
                    subscription.UserId == userId && subscription.SubscriptionId == subscriptionId);

                if (!subscriptionExist)
                {
                    db.UserSubscriptions.Add(new UserSubscription
                    {
                        UserId = userId,
                        SubscriptionId = subscriptionId.Value,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.MaxValue
                    });
                    await db.SaveChangesAsync();
                }
            }

            return true;
        }
    }
}