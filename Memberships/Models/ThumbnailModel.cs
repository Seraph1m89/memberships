using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Memberships.Models
{
    public class ThumbnailModel : IEquatable<ThumbnailModel>
    {
        public int ProductId { get; set; }

        public int SubscriptionId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TagText { get; set; }

        public string ImageUrl { get; set; }

        public string Link { get; set; }

        public string ContentTag { get; set; }

        public bool Equals(ThumbnailModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ProductId == other.ProductId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ThumbnailModel) obj);
        }

        public override int GetHashCode()
        {
            return ProductId;
        }

        public static bool operator ==(ThumbnailModel left, ThumbnailModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ThumbnailModel left, ThumbnailModel right)
        {
            return !Equals(left, right);
        }
    }
}