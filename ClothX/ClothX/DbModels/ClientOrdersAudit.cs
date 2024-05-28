using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class ClientOrdersAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsConfirmed { get; set; }
        public bool? IsDelivered { get; set; }
        public int? ClientId { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
        public string? Measurements { get; set; }
        public int? OrderType { get; set; }
        public int? Price { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
