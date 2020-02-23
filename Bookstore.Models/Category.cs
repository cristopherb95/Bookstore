using System;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Category Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

    }
}
