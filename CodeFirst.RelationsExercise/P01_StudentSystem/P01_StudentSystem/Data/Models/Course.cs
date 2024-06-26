using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            StudentsCourses = new HashSet<StudentCourse>();
            Resources = new HashSet<Resource>();
            Homeworks = new HashSet<Homework>();
        }

        [Key]
        public int CourseId { get; set; }

        [MaxLength(80)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; } 

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }


        public ICollection<StudentCourse> StudentsCourses { get; set; } = null!;

        public ICollection<Resource> Resources { get; set; } = null!;

        public ICollection<Homework> Homeworks { get; set; } = null!;
    }
}
