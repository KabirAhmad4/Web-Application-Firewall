using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class OrderFeature
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FeatureId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Feature Feature { get; set; } = null!;
        public virtual ClientOrder Order { get; set; } = null!;
    }
}
