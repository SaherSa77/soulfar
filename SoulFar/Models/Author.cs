using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SoulFar.Models
{
    public partial class Author
    {
        public Author()
        {
            this.Products = new HashSet<Product>();
        }
        [Key]
        public int AuthorID { get; set; }
        [Required, Display(Name = "Enter author first name.")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Enter author last name.")]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [NotMapped]
        public string Name
        {
            get { return this.FirstName + " " + this.LastName; }
        }
        public virtual ICollection<Product> Products { get; set; }
    }
}