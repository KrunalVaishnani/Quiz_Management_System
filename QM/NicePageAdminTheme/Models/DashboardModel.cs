namespace NicePageAdminTheme.Models
{
    public class DashboardModel
    {
        public List<DashboardCountsModel> Counts { get; set; }
        public List<RecentQuizModel> RecentQuizes { get; set; }
        public List<RecentQuestionModel> RecentQuestions { get; set; }
        public List<RecentUserModel> RecentUsers { get; set; }
        public List<TopActiveUserModel> TopActiveUsers { get; set; }
        public List<QuickLinksModel> NavigationLinks { get; set; }
    }
}
