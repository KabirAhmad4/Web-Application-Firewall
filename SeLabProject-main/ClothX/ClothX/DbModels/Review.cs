using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class Review
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; } = null!;
        public string? Response { get; set; }
        public DateTime? ResponseAddedOn { get; set; }
        public int? Rating { get; set; }

        public virtual UserProfile User { get; set; } = null!;
    }
}
