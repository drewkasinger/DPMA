using C971.Models;
using C971.Services;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.RegularExpressions;
using static C971.Services.DegreePlanService;

namespace C971.Views;

public partial class CoursesView : ContentPage
{
    private readonly int selectedTermId;
    private int courseId;
    public static int CourseCount { get; set; }
	public CoursesView(int termid)
	{
		InitializeComponent();

        selectedTermId = termid;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var getTerm = await DegreePlanService.Terms.GetSelectedTerm(selectedTermId);

        foreach ( var term in getTerm)
        {
            if((term.termName != null) && (term.termStart != null) && (term.termEnd != null))
            {
                termname.Text = term.termName;
                termstart.Date = DateTime.Parse(term.termStart);
                termend.Date = DateTime.Parse(term.termEnd);
            }
        }

        await RefreshTermsView();
    }

    public async void OpenDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (courses.SelectedItem != null)
        {
            if(e.CurrentSelection.Count > 0)
            {
                var selectedCourse = e.CurrentSelection[0] as CoursesModel;
                courseId = selectedCourse.courseId;

                await Navigation.PushAsync(new DetailedCourseView(courseId));
                courses.SelectedItem = null;
            }
        }
    }

    public void OnClick_AddCourse(object sender, EventArgs e)
    {
        CheckForCourses();
    }

    public async void OnClick_SaveTerm(object sender, EventArgs e)
    {
        if (TermValidation() != null)
        {
            await DisplayAlert("Error", TermValidation(), "Ok");
        }
        else
        {
            await DegreePlanService.Terms.UpdateTerm(selectedTermId, termname.Text, termstart.Date.ToShortDateString(), termend.Date.ToShortDateString());
        }
    }

    public async void OnClick_DeleteTerm(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this term?", "Yes", "No");

        if (selectedTermId != 1)
        {
            if (answer)
            {
                await DegreePlanService.Terms.DeleteTerm(selectedTermId);
                await Navigation.PopAsync();
            }
        }
        else
        {
            await DisplayAlert("Error", "You can not delete the example template", "ok");
        }
        
    }

    private async Task RefreshTermsView()
    {
        var getCourses = await DegreePlanService.Courses.GetTermCourses(selectedTermId);
        courses.ItemsSource = getCourses;
    }

    public async void CheckForCourses()
    {
        var courses = await DegreePlanService.Courses.GetTermCourses(selectedTermId);
        CourseCount = courses.Count();

        if (CourseCount < 6)
        {
            await Navigation.PushAsync(new AddCourseView(selectedTermId));
        }
        else
        {
            await DisplayAlert("To Many Courses", "You already have the maximum number of courses for this term.", "OK");
        }
    }

    public string TermValidation()
    {
        string textPattern = @"^[a-zA-Z0-9 ]+$";
        if (string.IsNullOrWhiteSpace(termname.Text))
        {
            return "Please enter a Term name";
        }
        else if (!Regex.IsMatch(termname.Text, textPattern))
        {
            return "Please only use letters and numbers, no special characters.";
        }
        else if (termstart.Date < DateTime.Now)
        {
            return "Term Start Date can not be in the past.";
        }
        else if (termstart.Date == DateTime.Now)
        {
            return "Term Start Date can not start today.";
        }
        else if (termend.Date < termstart.Date)
        {
            return "Term End Date can not be before Start Date.";
        }
        else
        {
            return null;
        }
    }
}