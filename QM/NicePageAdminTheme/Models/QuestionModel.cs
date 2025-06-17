using System.ComponentModel.DataAnnotations;

namespace NicePageAdminTheme.Models
{
    public class QuestionModel
    {
        public int QuestionID { get; set; } // Primary Key, Auto Increment
        [Required]
        [MaxLength(100)]
        public string QuestionText { get; set; } // Not Null

        [Required]
        public int QuestionLevelID { get; set; } // Foreign Key, Not Null

        [Required]
        [MaxLength(100)]
        public string OptionA { get; set; } // Not Null
        [Required]
        [MaxLength(100)]
        public string OptionB { get; set; } // Not Null
        [Required]
        [MaxLength(100)]
        public string OptionC { get; set; } // Not Null
        [Required]
        [MaxLength(100)]
        public string OptionD { get; set; } // Not Null
        [Required]
        [MaxLength(100)]
        public string CorrectOption { get; set; } // Not Null
        [Required]
        [Range(1,100)]
        [Display(Name = "Mark of question")]
        public int QuestionMarks { get; set; } // Not Null
        public bool IsActive { get; set; } = true; // Not Null, Default True

        public int UserID { get; set; } // Foreign Key, Not Null
    }

    public class QuestionLevelDropDownModel
    {
        public int QuestionLevelID { get; set; } // Primary Key
        public string QuestionLevel { get; set; } // Not Null
    }

    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
