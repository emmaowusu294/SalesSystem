using System.ComponentModel.DataAnnotations;

namespace SalesSystem.ViewModels
{
    public class CustomerViewModel
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
    }
}
