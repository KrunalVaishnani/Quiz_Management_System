using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Data.SqlClient;
using NicePageAdminTheme.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NicePageAdminTheme.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IConfiguration _configuration;

        public QuestionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult QuestionList()
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_Question_SelectAll";
            sqlCommand.Parameters.AddWithValue("@UserID", CommonVariable.UserID());

            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            sqlConnection.Close();
            return View(table);
        }

        public IActionResult QuestionDelete(int QuestionID)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "PR_MST_Question_Delete";
                sqlCommand.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;
                sqlCommand.ExecuteNonQuery();

                sqlConnection.Close();
                return RedirectToAction("QuestionList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("QuestionList");
            }
        }

        [Route("Question/QuestionForm")]
        public IActionResult QuestionForm(int? QuestionID)
        {
            string connectionString = this._configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();

            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_MST_QuestionLevel_DropDown"; // Your stored procedure for fetching question levels
            command1.Parameters.AddWithValue("@UserID", CommonVariable.UserID());

            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);

            List<QuestionLevelDropDownModel> levelList = new List<QuestionLevelDropDownModel>();

            foreach (DataRow data in dataTable1.Rows)
            {
                QuestionLevelDropDownModel questionLevelDropDownModel = new QuestionLevelDropDownModel();
                questionLevelDropDownModel.QuestionLevelID = Convert.ToInt32(data["QuestionLevelID"]);
                questionLevelDropDownModel.QuestionLevel = data["QuestionLevel"].ToString();
                levelList.Add(questionLevelDropDownModel);
            }

            ViewBag.LevelList = levelList; // Pass the list to the view

            //SqlConnection connection2 = new SqlConnection(connectionString);
            //connection2.Open();
            //SqlCommand command2 = connection2.CreateCommand();
            //command1.CommandType = System.Data.CommandType.StoredProcedure;
            //command1.CommandText = "PR_MST_User_DropDown";
            //SqlDataReader reader2 = command1.ExecuteReader();
            //DataTable dataTable2 = new DataTable();
            //dataTable2.Load(reader2);

            //List<UserDropDownModel> userList = new List<UserDropDownModel>();

            //foreach (DataRow data in dataTable2.Rows)
            //{
            //    UserDropDownModel userDropDownModel = new UserDropDownModel();
            //    userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
            //    userDropDownModel.UserName = data["UserName"].ToString();
            //    userList.Add(userDropDownModel);
            //}
            //ViewBag.UserList = userList;


            if (QuestionID == null || QuestionID == 0)
            {
                return View(new QuestionModel());
            }

            #region QuestionByID

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_Question_SelectByPK";
            command.Parameters.AddWithValue("@QuestionID", QuestionID);
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuestionModel questionModel = new QuestionModel();

            foreach (DataRow dataRow in table.Rows)
            {
                questionModel.QuestionID = Convert.ToInt32(dataRow["QuestionID"]);
                questionModel.QuestionText = dataRow["QuestionText"].ToString();
                questionModel.QuestionLevelID = Convert.ToInt32(dataRow["QuestionLevelID"]);
                questionModel.OptionA = dataRow["OptionA"].ToString();
                questionModel.OptionB = dataRow["OptionB"].ToString();
                questionModel.OptionC = dataRow["OptionC"].ToString();
                questionModel.OptionD = dataRow["OptionD"].ToString();
                questionModel.CorrectOption = dataRow["CorrectOption"].ToString();
                questionModel.QuestionMarks = Convert.ToInt32(dataRow["QuestionMarks"]);
                questionModel.IsActive = Convert.ToBoolean(dataRow["IsActive"]);
            }

            #endregion


            connection1.Close();
            connection.Close();

            return View(questionModel);
        }

        public IActionResult OnSubmit(QuestionModel questionModel)
        {
            //if (questionModel.UserID <= 0)
            //{
            //    ModelState.AddModelError("UserID", "A valid User is required.");
            //}

            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (questionModel.QuestionID <= 0)
                {
                    command.CommandText = "PR_MST_Question_Insert";
                }
                else
                {
                    command.CommandText = "PR_MST_Question_Update";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionModel.QuestionID;
                }

                command.Parameters.Add("@QuestionText", SqlDbType.NVarChar).Value = questionModel.QuestionText;
                command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = questionModel.QuestionLevelID;
                command.Parameters.Add("@OptionA", SqlDbType.NVarChar).Value = questionModel.OptionA;
                command.Parameters.Add("@OptionB", SqlDbType.NVarChar).Value = questionModel.OptionB;
                command.Parameters.Add("@OptionC", SqlDbType.NVarChar).Value = questionModel.OptionC;
                command.Parameters.Add("@OptionD", SqlDbType.NVarChar).Value = questionModel.OptionD;
                command.Parameters.Add("@CorrectOption", SqlDbType.NVarChar).Value = questionModel.CorrectOption;
                command.Parameters.Add("@QuestionMarks", SqlDbType.Int).Value = questionModel.QuestionMarks;
                
                if (questionModel.QuestionID > 0)
                {
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = questionModel.IsActive;
                }

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID();

                command.ExecuteNonQuery();

                connection.Close();
                return RedirectToAction("QuestionList");
            }

            return View("QuestionForm", questionModel);
        }
    }
}
