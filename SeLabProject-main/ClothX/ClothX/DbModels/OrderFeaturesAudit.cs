using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class OrderFeaturesAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public int? OrderId { get; set; }
        public int? FeatureId { get; set; }
        public bool? IsActive { get; set; }
    }
}
