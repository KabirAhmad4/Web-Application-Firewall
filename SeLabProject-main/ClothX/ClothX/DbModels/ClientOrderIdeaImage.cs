using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class ClientOrderIdeaImage
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ImagePath { get; set; } = null!;

        public virtual ClientOrder Order { get; set; } = null!;
    }
}
