using System.ComponentModel.DataAnnotations;
using P01_StudentSystem.Data.Models.Enums;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public ContentType ContentType { get; set; }

        [Required]
        public DateTime SubmissionTime { get; set; }

        public int StudentId { get; set; }
        public int CourseId { get; set; }

        public virtual Student Student { get; set; }

        public virtual Course Course { get; set; }
    }
}
