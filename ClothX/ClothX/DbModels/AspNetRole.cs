using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaim>();
            Preveliges = new HashSet<Prevelige>();
            Users = new HashSet<AspNetUser>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public string? ConcurrencyStamp { get; set; }

        public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; }

        public virtual ICollection<Prevelige> Preveliges { get; set; }
        public virtual ICollection<AspNetUser> Users { get; set; }
    }
}
