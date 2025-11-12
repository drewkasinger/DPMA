using C971.Models;
using C971.Services;
using C971.Views;
using System.Collections;
using System.Data;
using System.Runtime.CompilerServices;
using static C971.Services.DegreePlanService;

namespace Degree_Plan_Mobile_App.Views;

public partial class ReportsView : ContentPage
{
    private readonly int AccountId;
    private readonly DateTime StartDate;
    private readonly DateTime EndDate;
    private int CourseId;
    DataTable ReportCourses = new DataTable();
	public ReportsView(int accountid, DateTime start, DateTime end)
	{
		InitializeComponent();

        AccountId = accountid;
        StartDate = start;
        EndDate = end;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        GetReportCourses();
    }

    public async void GetReportCourses()
    {
        List<CoursesModel> reportCourses = new List<CoursesModel>();

        var getTerms = await DegreePlanService.Terms.GetTerms(AccountId);
        foreach (var term in getTerms)
        {
            var courses = await DegreePlanService.Courses.GetTermCourses(term.termId);
            foreach (var course in courses)
            {
                if (DateTime.Parse(course.courseStart).DayOfYear >= StartDate.DayOfYear && DateTime.Parse(course.courseStart).DayOfYear <= EndDate.DayOfYear)
                {
                    reportCourses.Add(course);
                }
            }
        }
        courses.ItemsSource = reportCourses;
    }

    public async void OpenDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (courses.SelectedItem != null)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedCourse = e.CurrentSelection[0] as CoursesModel;
                CourseId = selectedCourse.courseId;

                bool answer = await DisplayAlert("Warning", "You are about to leave this page", "Confirm", "Cancel");

                if (answer)
                {
                    await Navigation.PushAsync(new DetailedCourseView(CourseId));
                }

                courses.SelectedItem = null;
            }
        }
    }
}