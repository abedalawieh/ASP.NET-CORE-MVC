using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public string ISBN { get; set; }
        [Required]

        public string Author { get; set; }
        [Required]
        [Display(Name ="List Price")]
        [Range(1,1000)]

        public string ListPrice { get; set; }
        [Required]
        [Display(Name = "Price for 1-50 ")]
        [Range(1, 1000)]

        public string Price { get; set; }
        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]

        public string Price50 { get; set; }
        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]

        public string Price100 { get; set; }

    }
}
