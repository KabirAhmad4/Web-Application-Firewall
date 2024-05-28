using System;
using System.Collections.Generic;

namespace ClothX.DbModels
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            ClientOrders = new HashSet<ClientOrder>();
            TailorProjects = new HashSet<TailorProject>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Price { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; } = null!;
        public DateTime UpdatedOn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<ClientOrder> ClientOrders { get; set; }
        public virtual ICollection<TailorProject> TailorProjects { get; set; }
    }
}
