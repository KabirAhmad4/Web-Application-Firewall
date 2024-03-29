using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class Feature
    {
        public Feature()
        {
            OrderFeatures = new HashSet<OrderFeature>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int FeatureGroupId { get; set; }
        public int Price { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; } = null!;
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual FeatureGroup FeatureGroup { get; set; } = null!;
        public virtual ICollection<OrderFeature> OrderFeatures { get; set; }
    }
}
