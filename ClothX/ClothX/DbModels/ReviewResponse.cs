using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class ReviewResponse
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public int ReviewId { get; set; }
        public int ReviewBy { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; } = null!;
        public bool? IsActive { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual Review Review { get; set; } = null!;
        public virtual UserProfile ReviewByNavigation { get; set; } = null!;
    }
}
