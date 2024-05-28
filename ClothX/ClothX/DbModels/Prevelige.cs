using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class Prevelige
    {
        public Prevelige()
        {
            Roles = new HashSet<AspNetRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
