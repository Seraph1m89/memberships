using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Memberships.Entities
{
    [Table("SubscriptionProduct")]
    public class SubscriptionProduct
    {
        [Key, Column(Order = 1)]
        [Required]
        public int SubscriptionId { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        public int ProductId { get; set; }

        [NotMapped]
        public int OldSubscriptionId { get; set; }

        [NotMapped]
        public int OldProductId { get; set; }
    }
}