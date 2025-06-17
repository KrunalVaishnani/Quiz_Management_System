using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NicePageAdminTheme.Models;
using System.Data;
using System.Data.SqlClient;

namespace NicePageAdminTheme.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region List
        public IActionResult UserList()
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("PR_MST_User_SelectAll", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure,
                };
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);

            }
            return View(dt);
        }
        #endregion
        //#region AddEdit
        //public IActionResult UserForm(int? UserID)
        //{
        //    string connectionString = _configuration.GetConnectionString("ConnectionString");

        //    UserModel model = new UserModel();
        //    if (UserID.HasValue)
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            SqlCommand command = new SqlCommand("PR_MST_User_SelectByPK", connection)
        //            {
        //                CommandType = CommandType.StoredProcedure
        //            };
        //            command.Parameters.AddWithValue("@UserID", UserID.Value);
        //            SqlDataReader reader = command.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                model.UserID = Convert.ToInt32(reader["UserID"]);
        //                model.UserName = reader["UserName"].ToString();
        //                model.Email = reader["Email"].ToString();
        //                model.MobileNo = reader["Mobile"].ToString();
        //                model.Password = reader["Password"].ToString();
        //                model.IsActive = Convert.ToBoolean(reader["IsActive"]);
        //                model.IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);
        //            }
        //        }
        //    }
        //    return View("UserForm", model);
        //}

        //#endregion
        #region Save
        public IActionResult Save(UserModel model)
        {
            if (model.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "A valid User is required.");
            }



            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;

                    if (model.UserID == 0 || !model.UserID.HasValue) // Insert
                    {
                        command.CommandText = "PR_MST_User_Insert";
                    }
                    else // Update
                    {
                        command.CommandText = "PR_MST_User_Update";
                        command.Parameters.AddWithValue("@IsActive", model.IsActive);
                        command.Parameters.AddWithValue("@IsAdmin", model.IsAdmin);
                        command.Parameters.AddWithValue("@UserID", model.UserID);
                    }

                    command.Parameters.AddWithValue("@UserName", model.UserName);
                    command.Parameters.AddWithValue("@Password", model.Password);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Mobile", model.MobileNo);

                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Login");
            }

            return View("SignUp", model);
        }
        #endregion
        #region Delete
        public IActionResult Delete(int UserID)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("PR_MST_User_Delete", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                Console.WriteLine(ex.ToString());
            }

            return RedirectToAction("UserList");
        }
        #endregion

        #region UserLogin
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this._configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Login";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        #endregion

        #region UserLogOut
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
        #endregion

        #region LoginPage
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region SignUp
        public IActionResult SignUp(int? UserID)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString");

            UserModel model = new UserModel();
            if (UserID.HasValue)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("PR_MST_User_SelectByPK", connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@UserID", UserID.Value);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        model.UserID = Convert.ToInt32(reader["UserID"]);
                        model.UserName = reader["UserName"].ToString();
                        model.Email = reader["Email"].ToString();
                        model.MobileNo = reader["Mobile"].ToString();
                        model.Password = reader["Password"].ToString();
                        model.IsActive = Convert.ToBoolean(reader["IsActive"]);
                        model.IsAdmin = Convert.ToBoolean(reader["IsAdmin"]);
                    }
                }
            }
            return View("SignUp", model);
        }
        #endregion
    }
}
