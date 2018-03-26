using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Memberships.Models
{
    public class UserSubscriptionModel
    {
        [DisplayName("Id")]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(20)]
        public string RegistrationCode { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}