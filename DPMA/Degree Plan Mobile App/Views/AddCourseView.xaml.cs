using C971.Models;
using C971.Services;
using System.Text.RegularExpressions;

namespace C971.Views;

public partial class AddCourseView : ContentPage
{
    private readonly int selectedterm;
    private readonly string defaultstatus;
    public AddCourseView(int termid)
	{
		InitializeComponent();
        DateTime start = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("MM/01/yyyy"));
        newcoursestart.Date = start;
        newcourseend.Date = start.AddMonths(1).AddDays(-1);

        defaultstatus = "Not Started";
        selectedterm = termid;
    }

    public async void OnClick_AddCourse(object sender, EventArgs e)
    {
        if (CourseValidation() != null)
        {
            await DisplayAlert("Error", CourseValidation().ToString(), "Ok");
        }
        else
        {
            await DegreePlanService.Courses.AddCourses(selectedterm, defaultstatus, newcoursename.Text, newcoursestart.Date, newcourseend.Date);
            var courses = await DegreePlanService.Courses.GetTermCourses(selectedterm);
            var count = courses.Count();

            int i = 1;
            foreach (var course in courses)
            {
                if (count == i)
                {
                    await DegreePlanService.DetailedCourse.AddDetailedCourse(course.courseId, course.statusPicker, DateTime.Parse(course.courseEnd), DateTime.Parse(course.courseStart), DateTime.Parse(course.courseEnd).AddDays(-1), "", "", "", "");
                }
                i++;
            }

            await Navigation.PopAsync();
        }
    }

    public string CourseValidation()
    {
        string textPattern = @"^[a-zA-Z0-9 ]+$";
        if (string.IsNullOrWhiteSpace(newcoursename.Text))
        {
            return "Please enter a name";
        }
        else if (!Regex.IsMatch(newcoursename.Text, textPattern))
        {
            return "Please only use letters and numbers, no special characters.";
        }
        else if (newcoursestart.Date < DateTime.Now)
        {
            return "Start Date can not be in the past.";
        }
        else if (newcoursestart.Date == DateTime.Now)
        {
            return "Start Date can not start today.";
        }
        else if (newcourseend.Date < newcoursestart.Date)
        {
            return "End Date can not be before Start Date.";
        }
        else
        {
            return null;
        }
    }
}