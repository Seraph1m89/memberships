using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Memberships.Models
{
    public class ProductSection : IEquatable<ProductSection>
    {
        public int SectionId { get; set; }

        public string Title { get; set; }

        public int ItemTypeId { get; set; }

        public IEnumerable<ProductItemRow> ProductItemRows { get; set; }

        public bool Equals(ProductSection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SectionId == other.SectionId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProductSection) obj);
        }

        public override int GetHashCode()
        {
            return SectionId;
        }

        public static bool operator ==(ProductSection left, ProductSection right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProductSection left, ProductSection right)
        {
            return !Equals(left, right);
        }
    }
}