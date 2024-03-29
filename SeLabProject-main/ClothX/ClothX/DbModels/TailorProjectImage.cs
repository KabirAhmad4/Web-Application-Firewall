using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class TailorProjectImage
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ImagePath { get; set; } = null!;

        public virtual TailorProject Project { get; set; } = null!;
    }
}
