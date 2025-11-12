using C971.Services;
using System.Text.RegularExpressions;

namespace C971.Views;

public partial class EditOAView : ContentPage
{
    private readonly int CourseId;
    private readonly int OAID;
    private readonly string OAName;
    public EditOAView(int courseid, int oaid, string name)
	{
		InitializeComponent();

        CourseId = courseid;
        OAID = oaid;
        OAName = name;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var getassessment = await DegreePlanService.Assessments.GetOAAssessment(CourseId);

        foreach (var assessment in getassessment)
        {
            newassessment.Text = assessment.OA;
            startnotification.Date = assessment.OAStart;
            endnotification.Date = assessment.OAEnd;
        }
    }

    public async void OnClick_SaveAssessment(object sender, EventArgs e)
    {
        if (AssessmentValidation() != null)
        {
            await DisplayAlert("Error", AssessmentValidation(), "Ok");
        }
        else
        {
            if (OAName == "No OA Assessment has been added")
            {
                await DegreePlanService.Assessments.AddOA(CourseId, newassessment.Text, startnotification.Date.ToShortDateString(), endnotification.Date.ToShortDateString());

                await Navigation.PopAsync();
            }
            else
            {
                await DegreePlanService.Assessments.UpdateOAAssessment(OAID, newassessment.Text, startnotification.Date.ToShortDateString(), endnotification.Date.ToShortDateString());

                await Navigation.PopAsync();
            }
        }
    }

    public async void OnClick_DeleteAssessment(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this assessment?", "Yes", "No");

        if (answer)
        {
            await DegreePlanService.Assessments.DeleteOAAssessment(OAID);
            await Navigation.PopAsync();
        }
    }

    private string AssessmentValidation()
    {
        string textPattern = @"^[a-zA-Z0-9 ]+$";
        if (string.IsNullOrWhiteSpace(newassessment.Text))
        {
            return "Please enter an OA assessment name";
        }
        else if (!Regex.IsMatch(newassessment.Text, textPattern))
        {
            return "Please only use letters and numbers, no special characters.";
        }
        else if (startnotification.Date < DateTime.Now)
        {
            return "Start Notification Date can not be before todays date.";
        }
        else if (startnotification.Date == DateTime.Now)
        {
            return "Start Notification Date can not start today.";
        }
        else if (endnotification.Date < startnotification.Date)
        {
            return "End Notification Date can not be before Start Notification Date.";
        }
        return null;
    }
}