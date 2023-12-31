﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UniversityMGR_MVC.Models
{
    public class Course
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Course Name")]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public virtual ICollection<Group>? Groups { get; set; }

        public Course()
        {
            Groups = new List<Group>();
        }
    }
}
