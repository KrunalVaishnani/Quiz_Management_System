using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NicePageAdminTheme.Models;

namespace MVCQuizManagementSystem.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {
        private IConfiguration configuration;

        public HomeController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<IActionResult> Index()
        {
            var dashboardData = new DashboardModel
            {
                Counts = new List<DashboardCountsModel>(),
                RecentQuizes = new List<RecentQuizModel>(),
                RecentQuestions = new List<RecentQuestionModel>(),
                RecentUsers = new List<RecentUserModel>(),
                TopActiveUsers = new List<TopActiveUserModel>(),
                NavigationLinks = new List<QuickLinksModel>()
            };

            using (var connection = new SqlConnection(this.configuration.GetConnectionString("ConnectionString")))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_GetDashboardData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", CommonVariable.UserID());

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            // Fetch counts
                            while (await reader.ReadAsync())
                            {
                                dashboardData.Counts.Add(new DashboardCountsModel
                                {
                                    Metric = reader["Metric"].ToString(),
                                    Value = Convert.ToInt32(reader["Value"])
                                });
                            }

                            // Fetch recent orders
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.RecentQuizes.Add(new RecentQuizModel
                                    {
                                        QuizID = Convert.ToInt32(reader["QuizID"]),
                                        QuizName = reader["QuizName"].ToString(),
                                        TotalQuestions = Convert.ToInt32(reader["TotalQuestions"]),
                                        QuizDate = Convert.ToDateTime(reader["QuizDate"])
                                    });
                                }
                            }

                            // Fetch recent products
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.RecentQuestions.Add(new RecentQuestionModel
                                    {
                                        QuestionID = Convert.ToInt32(reader["QuestionID"]),
                                        QuestionText = reader["QuestionText"].ToString(),
                                        QuestionMarks = Convert.ToInt32(reader["QuestionMarks"]),
                                        CorrectOption = reader["CorrectOption"].ToString()
                                    });
                                }
                            }

                            // Fetch top customers
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.RecentUsers.Add(new RecentUserModel
                                    {
                                        UserID = Convert.ToInt32(reader["UserID"]),
                                        UserName = reader["UserName"].ToString(),
                                        Email = reader["Email"].ToString(),
                                        Mobile = reader["Mobile"].ToString()
                                    });
                                }
                            }

                            // Fetch top selling products
                            if (await reader.NextResultAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    dashboardData.TopActiveUsers.Add(new TopActiveUserModel
                                    {
                                        UserName = reader["UserName"].ToString(),
                                        TotalQuizes = Convert.ToInt32(reader["TotalQuizes"]),
                                        TotalQuestions = Convert.ToInt32(reader["TotalQuestions"])
                                    });
                                }
                            }
                        }
                    }
                }
            }

            dashboardData.NavigationLinks = new List<QuickLinksModel> {
                new QuickLinksModel {ActionMethodName = "Index", ControllerName="Dashboard", LinkName="Dashboard" },
                new QuickLinksModel {ActionMethodName = "Index", ControllerName="Quiz", LinkName="QuizList" },
                new QuickLinksModel {ActionMethodName = "Index", ControllerName="Question", LinkName="QuestionList" },
                new QuickLinksModel {ActionMethodName = "Index", ControllerName="QuestionLevel", LinkName="QuestionLevelList" },
                new QuickLinksModel {ActionMethodName = "Index", ControllerName="QuizWiseQuestion", LinkName="QuizWiseQuestionList" },
                new QuickLinksModel {ActionMethodName = "Index", ControllerName="User", LinkName="UserList" }
            };

            var model = new DashboardModel
            {
                Counts = dashboardData.Counts,
                RecentQuizes = dashboardData.RecentQuizes,
                RecentQuestions = dashboardData.RecentQuestions,
                RecentUsers = dashboardData.RecentUsers,
                TopActiveUsers = dashboardData.TopActiveUsers,
                NavigationLinks = dashboardData.NavigationLinks
            };

            return View(model);
        }
    }
}
