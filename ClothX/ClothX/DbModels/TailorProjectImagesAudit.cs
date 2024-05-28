using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class TailorProjectImagesAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public int? ProjectId { get; set; }
        public string? ImagePath { get; set; }
    }
}
