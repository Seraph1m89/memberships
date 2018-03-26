using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Memberships.Models;

namespace Memberships.Extensions
{
    public static class IdentityExtensions
    {
        public static async Task<string> GetUserFirstName(this IIdentity identity)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.UserName.Equals(identity.Name));
                return user?.FirstName ?? string.Empty;
            }
        }

        public static void FillUsers(this List<UserViewModel> users)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var dbUser = db.Users.AsEnumerable().Select(Mapper.Map<UserViewModel>).ToList();
                users.AddRange(dbUser);
            }
        }
    }
}