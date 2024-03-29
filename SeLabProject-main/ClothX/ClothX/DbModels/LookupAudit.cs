using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class LookupAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? Category { get; set; }
        public string? Value { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
