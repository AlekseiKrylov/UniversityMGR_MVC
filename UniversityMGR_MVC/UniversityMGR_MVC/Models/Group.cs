using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityMGR_MVC.Models
{
    public class Group
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        [Required]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        [HiddenInput(DisplayValue = false)]
        public Course? Course { get; set; }

        [HiddenInput(DisplayValue = false)]
        public virtual ICollection<Student>? Students { get; set; }

        public Group()
        {
            Students = new List<Student>();
        }
    }
}
