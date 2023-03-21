using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Task9.Models
{
    public class Student
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        
        public int? GroupId { get; set; }
        
        [HiddenInput(DisplayValue = false)]
        public Group? Group { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
