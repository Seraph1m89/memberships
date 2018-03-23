using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Memberships.Entities;

namespace Memberships.Areas.Admin.Models
{
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string Title { get; set; }

        [MaxLength(1024)]
        [DisplayName("Image URL")]
        public string ImageUrl { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        public int ProductLinkTextId { get; set; }

        public int ProductTypeId { get; set; }

        [DisplayName("Product Link Texts")]
        public ICollection<ProductLinkText> ProductLinkTexts { get; set; }

        [DisplayName("Product Types")]
        public ICollection<ProductType> ProductTypes { get; set; }

        [DisplayName("Product Type")]
        public string ProductType
        {
            get { return ProductTypes?.FirstOrDefault(p => p.Id == ProductTypeId)?.Title ?? string.Empty; }
        }

        [DisplayName("Product Link Text")]
        public string ProductLinkText
        {
            get { return ProductLinkTexts?.FirstOrDefault(p => p.Id == ProductLinkTextId)?.Title ?? string.Empty; }
        }
    }
}