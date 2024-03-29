using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class ClientOrderIdeaImagesAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public int? OrderId { get; set; }
        public string? ImagePath { get; set; }
    }
}
