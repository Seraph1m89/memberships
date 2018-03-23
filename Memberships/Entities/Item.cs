﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Memberships.Entities
{
    [Table("Item")]
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [MaxLength(1024)]
        public string Url { get; set; }

        [MaxLength(1024)]
        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }

        [AllowHtml]
        public string Html { get; set; }

        [DefaultValue(0)]
        [DisplayName("Wait Days")]
        public int WaitDays { get; set; }

        public string HtmlShort => Html == null || Html.Length < 50 ? Html : Html.Substring(0, 50);

        public int ProductId { get; set; }

        public int ItemTypeId { get; set; }

        public int PartId { get; set; }

        public int SectionId { get; set; }

        [DisplayName("Is Free")]
        public bool IsFree { get; set; }

        [DisplayName("Item Types")]
        public ICollection<ItemType> ItemTypes { get; set; }

        public ICollection<Section> Sections { get; set; }

        public ICollection<Part> Parts { get; set; }
    }
}