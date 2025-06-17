using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using NicePageAdminTheme.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace NicePageAdminTheme.Controllers
{
    public class QuizController : Controller
    {
        private IConfiguration _configuration;
        public QuizController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region List
        public IActionResult QuizList()
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_MST_Quiz_SelectAll", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,

                };
                cmd.Parameters.AddWithValue("@UserID", CommonVariable.UserID());

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                
            }
            return View(dt);
        }
        #endregion

        #region AddEdit
        public IActionResult QuizForm(int? QuizID)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");                

            QuizModel model = new QuizModel();
            if (QuizID.HasValue)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("PR_MST_Quiz_SelectByPK", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@QuizID", QuizID.Value);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        model.QuizID = Convert.ToInt32(reader["QuizID"]);
                        model.QuizName = reader["QuizName"].ToString();
                        model.TotalQuestions = Convert.ToInt32(reader["TotalQuestions"]);
                        model.QuizDate = Convert.ToDateTime(reader["QuizDate"]);
                    }
                }
            }
            //PopulateUserList();
            return View("QuizForm", model);
        }

        #endregion
        #region Save
        public IActionResult Save(QuizModel model)
        {
            // Ensure valid Created and Modified dates
            if (model.QuizDate == DateTime.MinValue)
            {
                model.QuizDate = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;

                    if (model.QuizID == 0) // Insert
                    {
                        command.CommandText = "PR_MST_Quiz_Insert";
                    }
                    else // Update
                    {
                        command.CommandText = "PR_MST_Quiz_Update";
                        command.Parameters.AddWithValue("@QuizID", model.QuizID);
                    }

                    command.Parameters.AddWithValue("@QuizName", model.QuizName);
                    command.Parameters.AddWithValue("@QuizDate", model.QuizDate);
                    command.Parameters.AddWithValue("@TotalQuestions", model.TotalQuestions);
                    command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());

                    command.ExecuteNonQuery();
                }

                return RedirectToAction("QuizList");
            }

            // Re-populate ViewBag.UserList for dropdown if validation fails
            //PopulateUserList();
            return View("QuizForm", model);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int QuizID)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("PR_MST_Quiz_Delete", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@QuizID", QuizID);
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("QuizList");
        }
        #endregion
    }
}
