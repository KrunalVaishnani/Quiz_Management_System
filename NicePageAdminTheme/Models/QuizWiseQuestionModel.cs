
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace NicePageAdminTheme.Models
{
    public class QuizWiseQuestionModel
    {
        public int QuizWiseQuestionsID { get; set; } // Primary Key, AutoIncrement

        public int QuizID { get; set; } // Foreign Key to Quiz table

        public string? QuizName { get; set; } // Related field for display

        public int QuestionID { get; set; } // Foreign Key to Question table

        public string? QuestionText { get; set; } // Related field for display

        public int UserID { get; set; } // Foreign Key to User table

        public string? UserName { get; set; } // Related field for display

        public DateTime Created { get; set; } // Automatically set to current date/time

        public DateTime Modified { get; set; } // Updated on modification


        [ValidateNever]
        public List<QuizDropDownModel> QuizList { get; set; }
        [ValidateNever]
        public List<QuestionModel> QuestionList { get; set; }
        [ValidateNever]
        public List<UsersDropDownModel> UserList { get; set; } // User Dropdown List
    }
    public class QuizDropDownModel
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }
    }

    //public class QuestionDropDownModel
    //{
    //    public int QuestionID { get; set; }
    //    public string QuestionText { get; set; }


    //}

    public class UsersDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

}
