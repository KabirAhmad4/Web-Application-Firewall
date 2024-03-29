using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class FeaturesAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? FeatureGroupId { get; set; }
        public int? Price { get; set; }
        public DateTime? AddedOn { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
