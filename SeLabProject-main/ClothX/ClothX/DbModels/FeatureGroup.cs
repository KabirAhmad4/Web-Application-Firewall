using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class FeatureGroup
    {
        public FeatureGroup()
        {
            Features = new HashSet<Feature>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; } = null!;
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Feature> Features { get; set; }
    }
}
