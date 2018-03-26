using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Memberships.Models
{
    public class UserViewModel
    {
        [DisplayName("User ID")]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(100)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}