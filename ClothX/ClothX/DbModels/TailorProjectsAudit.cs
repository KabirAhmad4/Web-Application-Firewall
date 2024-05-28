using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class TailorProjectsAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ProductCategoryId { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? AddedBy { get; set; }
        public bool? IsActive { get; set; }
        public string? ImagePath { get; set; }
    }
}
