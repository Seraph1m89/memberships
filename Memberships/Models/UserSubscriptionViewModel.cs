using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Memberships.Entities;

namespace Memberships.Models
{
    public class UserSubscriptionViewModel
    {
        [DisplayName("Available Subscriptions")]
        public ICollection<Subscription> AttachableSubscriptions { get; set; }

        public ICollection<UserSubscriptionModel> UserSubscriptions { get; set; }

        public bool DisableDropDown { get; set; }

        public string UserId { get; set; }

        public int SubscriptionId { get; set; }
    }
}