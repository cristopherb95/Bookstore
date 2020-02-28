﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class CoverType
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Cover Type")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

    }
}
