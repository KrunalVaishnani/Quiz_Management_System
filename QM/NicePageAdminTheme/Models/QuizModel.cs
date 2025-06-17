using System.ComponentModel.DataAnnotations;

namespace NicePageAdminTheme.Models
{
    public class QuizModel
    {
        public int QuizID { get; set; }
        [Required]
        public string QuizName { get; set; }
        [Required]
        [Range(5,50)]
        public int TotalQuestions { get; set; }
        [Required]
        public DateTime QuizDate { get; set; }
        [Required]
        public int UserID { get; set; }
    }
}
