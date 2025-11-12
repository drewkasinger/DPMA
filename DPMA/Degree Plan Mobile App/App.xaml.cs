using C971.Services;
using C971.Views;
using Degree_Plan_Mobile_App.Views;

namespace Degree_Plan_Mobile_App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //uncomment below code to delete all tables and reinitialize them, use if any models data is modified.
            //DeleteTable();

            var loginView = new LoginView();
            var navPage = new NavigationPage(loginView);
            MainPage = navPage;
        }

        private async void DeleteTable()
        {
            await DegreePlanService.DeleteTables();
            await DegreePlanService.CreateTestAccount();
        }
    }
}
