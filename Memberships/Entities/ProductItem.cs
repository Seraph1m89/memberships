using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Memberships.Entities
{
    [Table("ProductItem")]
    public class ProductItem
    {
        [Key, Column(Order = 1)]
        [Required]
        public int ProductId { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        public int ItemId { get; set; }
        [NotMapped]
        public int OldItemId { get; set; }
        [NotMapped]
        public int OldProductId { get; set; }
    }
}