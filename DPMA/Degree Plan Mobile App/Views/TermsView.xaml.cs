using C971.Models;
using C971.Services;
using Degree_Plan_Mobile_App.Views;

namespace C971.Views;

public partial class TermsView : ContentPage
{
    public static int TermCount { get; set; }
	private readonly int AccountID;
	public TermsView(int accountId, string logincheck)
	{
		InitializeComponent();

		AccountID = accountId;

		NotificationChecker();
    }

	public async void OnClick_AddTerm(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddTermView(AccountID));
	}
	public async void OnClick_RunReport(object sender, EventArgs e)
	{
        if (ReportValidation() != null)
        {
            await DisplayAlert("Error", ReportValidation(), "Ok");
        }
		else
		{
            await Navigation.PushAsync(new ReportsView(AccountID, start.Date, end.Date));
        }
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var terms = await DegreePlanService.Terms.GetTerms(AccountID);
		int termcount = terms.Count();
		if ((AccountID == 1) && (termcount == 0))
		{
			await DegreePlanService.LoadSampleData();
		}
        await RefreshTermsView();
    }

	private async Task RefreshTermsView()
	{
		var getTerms = await DegreePlanService.Terms.GetTerms(AccountID);
		terms.ItemsSource = getTerms;
	}

	private async void OpenCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if(terms.SelectedItem != null)
		{
			if (e.CurrentSelection.Count > 0)
			{
				var selectedTerm = e.CurrentSelection[0] as TermsModel;
				int termid = selectedTerm.termId;

				await Navigation.PushAsync(new CoursesView(termid));
				terms.SelectedItem = null;
			}
		}
	}

	private string ReportValidation()
	{
		if(start.Date > end.Date)
		{
			return "End Date must be the same as, or after the start date.";
		}
		return null;
	}


    public async void NotificationChecker()
	{
        var getTerms = await DegreePlanService.Terms.GetTerms(AccountID);
		foreach (var term in getTerms)
		{
			var termid = term.termId;
			var getCourses = await DegreePlanService.Courses.GetTermCourses(termid);
			foreach (var course in getCourses)
			{
				var courseid = course.courseId;
				var courseDetails = await DegreePlanService.DetailedCourse.GetCourseDetails(courseid);
				foreach (var detail in courseDetails)
				{
					if ((DateTime.Now >= detail.startNotification.Date) && !(DateTime.Now.DayOfYear > DateTime.Parse(course.courseStart).DayOfYear))
					{
						int start = DateTime.Parse(course.courseStart).DayOfYear - DateTime.Now.DayOfYear;
						await DisplayAlert("Warning", $"{term.termName}, Course {course.courseName} is starting in {start} day(s)", "Ok");
					}
					if ((DateTime.Now >= detail.endNotification.Date) && !(DateTime.Now.DayOfYear > DateTime.Parse(course.courseEnd).DayOfYear))
					{
						int end = DateTime.Parse(course.courseEnd).DayOfYear - DateTime.Now.DayOfYear;
						await DisplayAlert("Warning", $"{term.termName}, Course {course.courseName} is ending in {end} day(s)", "Ok");
					}

					var OaAssessment = await DegreePlanService.Assessments.GetOAAssessment(courseid);
					foreach (var OA in OaAssessment)
					{
						if ((DateTime.Now >= OA.OAStart.Date) && !(DateTime.Now > DateTime.Parse(course.courseStart)))
						{
							int start = DateTime.Parse(course.courseStart).DayOfYear - DateTime.Now.DayOfYear;
							await DisplayAlert("Warning", $"{term.termName}, Course {course.courseName}, Assessment {OA.OA} is starting in {start} day(s)", "Ok");
						}
						if ((DateTime.Now >= OA.OAEnd.Date) && !(DateTime.Now > DateTime.Parse(course.courseEnd)))
						{
							int end = DateTime.Parse(course.courseEnd).DayOfYear - DateTime.Now.DayOfYear;
							await DisplayAlert("Warning", $"{term.termName}, Course {course.courseName}, Assessment {OA.OA} is starting in {end} day(s)", "Ok");
						}

					}

					var PaAssessment = await DegreePlanService.Assessments.GetPAAssessment(courseid);
					foreach (var PA in PaAssessment)
					{
						if ((DateTime.Now >= PA.PAStart.Date) && !(DateTime.Now > DateTime.Parse(course.courseStart)))
						{
							int start = DateTime.Parse(course.courseStart).DayOfYear - DateTime.Now.DayOfYear;
							await DisplayAlert("Warning", $"{term.termName}, Course {course.courseName}, Assessment {PA.PA} is starting in {start} day(s)", "Ok");
						}
						if ((DateTime.Now >= PA.PAEnd.Date) && !(DateTime.Now > DateTime.Parse(course.courseEnd)))
						{
							int end = DateTime.Parse(course.courseEnd).DayOfYear - DateTime.Now.DayOfYear;
							await DisplayAlert("Warning", $"{term.termName}, Course {course.courseName}, Assessment {PA.PA} is starting in {end} day(s)", "Ok");
						}
					}
				}
			}
		}
	}
}