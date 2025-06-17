using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using NicePageAdminTheme.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Windows.Input;
namespace NicePageAdminTheme.Controllers
{
    public class QuizWiseQuestionController : Controller
    {
        #region IConfiguration
        private readonly IConfiguration configuration;

        public QuizWiseQuestionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region QuizWiseQuestionList
        public IActionResult QuizWiseQuestionList()
        {
            DataTable table = new DataTable();
            string connectionString = configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_QuizWiseQuestions_SelectAll";
                command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());

                SqlDataReader reader = command.ExecuteReader();
                table.Load(reader); // Populate DataTable with data from reader
            }
            return View(table); // Return DataTable to the view
        }

        #endregion

        
        #region QuizWiseQuestionAddEdit
        public IActionResult QuizWiseQuestionAddEdit(int QuizWiseQuestionsID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            // Fetch QuizWiseQuestion data
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_QuizWiseQuestions_SelectByPK";
            command.Parameters.AddWithValue("@QuizWiseQuestionsID", QuizWiseQuestionsID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            
            QuizWiseQuestionModel quizWiseQuestionModel = new QuizWiseQuestionModel();

            quizWiseQuestionModel.QuizList = GetQuizDropDown();
            quizWiseQuestionModel.QuestionList = GetQuestionDropDown();
           // quizWiseQuestionModel.UserList = GetUserDropDown();

            foreach (DataRow dataRow in table.Rows)
            {
                quizWiseQuestionModel.QuizWiseQuestionsID = Convert.ToInt32(dataRow["QuizWiseQuestionsID"]);
                quizWiseQuestionModel.QuizID = Convert.ToInt32(dataRow["QuizID"]);
                quizWiseQuestionModel.QuestionID = Convert.ToInt32(dataRow["QuestionID"]);
                quizWiseQuestionModel.QuizName = quizWiseQuestionModel.QuizList.FirstOrDefault(u => u.QuizID == quizWiseQuestionModel.QuizID).QuizName;
                quizWiseQuestionModel.QuestionText = quizWiseQuestionModel.QuestionList.FirstOrDefault(u => u.QuestionID == quizWiseQuestionModel.QuestionID).QuestionText;
                //quizWiseQuestionModel.UserName = quizWiseQuestionModel.UserList.FirstOrDefault(u => u.UserID == quizWiseQuestionModel.UserID).UserName;
            }

            // Fetch dropdown data
           
            return View("QuizWiseQuestionAddEdit", quizWiseQuestionModel);
        }
        #endregion

        #region QuizWiseQuestionSave

        [HttpPost]
        public IActionResult QuizWiseQuestionSave(QuizWiseQuestionModel quizWiseQuestionModel , List<int> SelectedQuestions)
        {
           

            if ((SelectedQuestions == null || !SelectedQuestions.Any()) && quizWiseQuestionModel.QuizWiseQuestionsID==0)
            {
                ModelState.AddModelError("SelectedQuestions", "At least one question must be selected.");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");

                if (quizWiseQuestionModel.QuizWiseQuestionsID == 0)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        foreach (var questionId in SelectedQuestions)
                        {
                            using (SqlCommand command = connection.CreateCommand())
                            {
                                command.CommandType = CommandType.StoredProcedure;

                            
                                    command.CommandText = "PR_MST_QuizWiseQuestions_Insert";
                            

                                command.Parameters.Add("@QuizID", SqlDbType.Int).Value = quizWiseQuestionModel.QuizID;
                                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionId;
                                command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID();

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "PR_MST_QuizWiseQuestions_Update";
                            command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = quizWiseQuestionModel.QuizWiseQuestionsID;
                            command.Parameters.Add("@QuizID", SqlDbType.Int).Value = quizWiseQuestionModel.QuizID;
                            command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = quizWiseQuestionModel.QuestionID;
                            command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID();

                            command.ExecuteNonQuery();
                        }
                    }
                }

                return RedirectToAction("QuizWiseQuestionList");
            }
            

            quizWiseQuestionModel.QuizList = GetQuizDropDown();
            quizWiseQuestionModel.QuestionList = GetQuestionDropDown();
            //quizWiseQuestionModel.UserList = GetUserDropDown();

            return View("QuizWiseQuestionAddEdit", quizWiseQuestionModel);
        }
        #endregion

        #region GetQuizDropDown

        public List<QuizDropDownModel> GetQuizDropDown()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Quiz_DropDown";
                command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());
                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                List<QuizDropDownModel> quizList = new List<QuizDropDownModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    QuizDropDownModel quizDropDownModel = new QuizDropDownModel
                    {
                        QuizID = Convert.ToInt32(dataRow["QuizID"]),
                        QuizName = dataRow["QuizName"].ToString()
                    };
                    quizList.Add(quizDropDownModel);
                }

                return quizList;
            }
        }

        #endregion

        #region GetQuestionDropDown

        public List<QuestionModel> GetQuestionDropDown()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_MST_Question_SelectAll";
                command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());

                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                List<QuestionModel> questionList = new List<QuestionModel>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    QuestionModel questionDropDownModel = new QuestionModel
                    {
                        QuestionID = Convert.ToInt32(dataRow["QuestionID"]),
                        QuestionText = dataRow["QuestionText"].ToString(),
                        OptionA = dataRow["OptionA"].ToString(),
                        OptionB = dataRow["OptionB"].ToString(),
                        OptionC = dataRow["OptionC"].ToString(),
                        OptionD = dataRow["OptionD"].ToString(),
                        CorrectOption = dataRow["CorrectOption"].ToString(),
                        QuestionMarks = Convert.ToInt32(dataRow["QuestionMarks"])

                    };
                    questionList.Add(questionDropDownModel);
                }

                return questionList;
            }
        }

        #endregion

        //public List<UsersDropDownModel> GetUserDropDown()
        //{
        //    string connectionString = configuration.GetConnectionString("ConnectionString");
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "PR_User_DropDown";  // Assuming you have this SP to get user details
        //        SqlDataReader reader = command.ExecuteReader();
        //        DataTable dataTable = new DataTable();
        //        dataTable.Load(reader);

        //        List<UsersDropDownModel> userList = new List<UsersDropDownModel>();
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            UsersDropDownModel userDropDownModel = new UsersDropDownModel
        //            {
        //                UserID = Convert.ToInt32(dataRow["UserID"]),
        //                UserName = dataRow["UserName"].ToString()
        //            };
        //            userList.Add(userDropDownModel);
        //        }

        //        return userList;
        //    }
        //}

        #region QuizWiseQuestionDelete
        public IActionResult QuizWiseQuestionDelete(int QuizWiseQuestionsID)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            // Fetch QuizWiseQuestion data
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_QuizWiseQuestions_Delete";
            command.Parameters.AddWithValue("@QuizWiseQuestionsID", QuizWiseQuestionsID);

            command.ExecuteNonQuery();
            return RedirectToAction("QuizWiseQuestionList");
        }
        #endregion
    }
}
