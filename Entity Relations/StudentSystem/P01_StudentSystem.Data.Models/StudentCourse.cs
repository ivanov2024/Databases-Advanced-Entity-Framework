using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class StudentCourse
    {
        [Key]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Key]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
