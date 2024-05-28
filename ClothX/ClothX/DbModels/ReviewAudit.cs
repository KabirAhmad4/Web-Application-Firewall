using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class ReviewAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? Message { get; set; }
        public int? UserId { get; set; }
        public DateTime? AddedOn { get; set; }
        public string? AddedBy { get; set; }
        public string? Response { get; set; }
        public DateTime? ResponseAddedOn { get; set; }
        public int? Rating { get; set; }
    }
}
