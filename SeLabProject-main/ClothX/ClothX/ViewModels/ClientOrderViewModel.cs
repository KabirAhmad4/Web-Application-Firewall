using ClothX.DbModels;
using System.ComponentModel.DataAnnotations;

namespace ClothX.ViewModels
{
    public class ClientOrderViewModel
    {
        public ClientOrderViewModel()
        {
            ClientOrderIdeaImages = new HashSet<ClientOrderIdeaImage>();
            OrderFeatures = new HashSet<OrderFeature>();
        }
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPaid { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDelivered { get; set; }
        public int ClientId { get; set; }
        public string AddedBy { get; set; } = null!;
        public DateTime AddedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsActive { get; set; }
        [Required]
        public string? Measurements { get; set; }
        [Required]
        public int OrderType { get; set; }
        [Required]
        public DateTime Deadline { get; set; }

        public IFormFileCollection? ideaImages { get; set; }
        [Required]
        [Range(1, 100000)]
        public int Price { get; set; }

        public virtual UserProfile Client { get; set; } = null!;
        public virtual ICollection<ClientOrderIdeaImage> ClientOrderIdeaImages { get; set; }
        public virtual ICollection<OrderFeature> OrderFeatures { get; set; }
    }
}
