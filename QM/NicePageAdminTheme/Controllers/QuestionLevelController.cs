using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using NicePageAdminTheme.Models;
using Microsoft.Data.SqlClient;

namespace NicePageAdminTheme.Controllers
{
    public class QuestionLevelController : Controller
    {
        public IConfiguration Configuration { get; }

        public QuestionLevelController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AddEdit
        public IActionResult AddEditQuestionLevel(int? QuestionLevelID)
        {
            string connectionString = Configuration.GetConnectionString("ConnectionString");

            // Load QuestionLevel Data for Editing
            QuestionLevelModel model = new QuestionLevelModel();
            if (QuestionLevelID.HasValue)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("PR_MST_QuestionLevel_SelectByPK", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@QuestionLevelID", QuestionLevelID.Value);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        model.QuestionLevelID = Convert.ToInt32(reader["QuestionLevelID"]);
                        model.QuestionLevel = reader["QuestionLevel"].ToString();
                        //model.Created = Convert.ToDateTime(reader["Created"]);
                        //model.Modified = Convert.ToDateTime(reader["Modified"]);
                    }
                }
            }

            return View("AddEditQuestionLevel", model);
        }

        #endregion
        #region Save
        public IActionResult SaveQuestionLevel(QuestionLevelModel model)
        {
            //if (CommonVariable.UserID() <= 0)
            //{
            //    ModelState.AddModelError("UserID", "A valid User is required.");
            //}

            // Ensure valid Created and Modified dates
            if (model.Created == DateTime.MinValue)
            {
                model.Created = DateTime.Now;
            }

            if (model.Modified == DateTime.MinValue)
            {
                model.Modified = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                string connectionString = Configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;

                    if (model.QuestionLevelID == null) // Insert
                    {
                        command.CommandText = "PR_MST_QuestionLevel_Insert";
                    }
                    else // Update
                    {
                        command.CommandText = "PR_MST_QuestionLevel_Update";
                        command.Parameters.AddWithValue("@QuestionLevelID", model.QuestionLevelID);
                    }

                    command.Parameters.AddWithValue("@QuestionLevel", model.QuestionLevel);
                    command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());

                    command.ExecuteNonQuery();
                }

                return RedirectToAction("QuestionLevelList");
            }

            // Re-populate ViewBag.UserList for dropdown if validation fails
            //PopulateUserList();
            return View("AddEditQuestionLevel", model);
        }
        #endregion


        #region List
        public IActionResult QuestionLevelList()
        {
            string connectionString = Configuration.GetConnectionString("ConnectionString");
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_MST_QuestionLevel_SelectAll", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserID", CommonVariable.UserID());
                SqlDataReader reader = command.ExecuteReader();
                dataTable.Load(reader);
            }

            return View(dataTable);
        }
        #endregion

        #region Delete
        public IActionResult DeleteQuestionLevel(int QuestionLevelID)
        {
            try
            {
                string connectionString = Configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("PR_MST_QuestionLevel_Delete", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@QuestionLevelID", QuestionLevelID);
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("QuestionLevelList");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("QuestionLevelList");
            }
        }
        #endregion

        [HttpPost]
        public IActionResult DeleteMultipleQuestionLevels(int[] SelectedQuestions)
        {
            if (SelectedQuestions == null || SelectedQuestions.Length == 0)
            {
                TempData["ErrorMessage"] = "No items selected for deletion.";
                return RedirectToAction("QuestionLevelList");
            }

            try
            {
                string connectionString = Configuration.GetConnectionString("ConnectionString");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (var id in SelectedQuestions)
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "PR_MST_QuestionLevel_Delete"; // Ensure this SP exists
                            command.Parameters.AddWithValue("@QuestionLevelID", id);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                TempData["SuccessMsg"] = "Selected question levels deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("QuestionLevelList");
        }
    }
}
