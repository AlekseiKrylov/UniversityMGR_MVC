using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Task9.Models
{
    public class Group
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Group Name")]
        public string Name { get; set; }
        
        [Required]
        public int CourseId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Course? Course { get; set; }

        [HiddenInput(DisplayValue = false)]
        public ICollection<Student> Students { get; set; }
        
         public Group()
        {
            Students = new List<Student>();
        }
    }
}
