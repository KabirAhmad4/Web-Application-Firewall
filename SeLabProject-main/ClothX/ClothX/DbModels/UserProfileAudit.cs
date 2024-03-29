using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class UserProfileAudit
    {
        public int AuditId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string? AuditUser { get; set; }
        public string? AuditOperation { get; set; }
        public int? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? AddedOn { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UserId { get; set; }
        public string? ImagePath { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
