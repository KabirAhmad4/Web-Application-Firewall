using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class Lookup
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string Value { get; set; } = null!;
        public int? DisplayOrder { get; set; }
    }
}
