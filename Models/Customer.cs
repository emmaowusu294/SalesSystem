using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesSystem.Models
{
    public class Customer
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

        //  A Customer can have many Sales
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}