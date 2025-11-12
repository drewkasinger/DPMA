using C971.Services;
using System.Text.RegularExpressions;

namespace C971.Views;

public partial class EditPAView : ContentPage
{
    private readonly int CourseId;
    private readonly int PAID;
    private readonly string PAName;
    public EditPAView(int courseid, int paid, string name)
	{
		InitializeComponent();

        CourseId = courseid;
        PAID = paid;
        PAName = name;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var getassessment = await DegreePlanService.Assessments.GetPAAssessment(CourseId);

        foreach (var assessment in getassessment)
        {
            newassessment.Text = assessment.PA;
            startnotification.Date = assessment.PAStart;
            endnotification.Date = assessment.PAEnd;
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
            if (PAName == "No PA Assessment has been added")
            {
                await DegreePlanService.Assessments.AddPA(CourseId, newassessment.Text, startnotification.Date.ToShortDateString(), endnotification.Date.ToShortDateString());

                await Navigation.PopAsync();
            }
            else
            {
                await DegreePlanService.Assessments.UpdatePAAssessment(PAID, newassessment.Text, startnotification.Date.ToShortDateString(), endnotification.Date.ToShortDateString());

                await Navigation.PopAsync();
            }
        }
    }

    public async void OnClick_DeleteAssessment(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this assessment?", "Yes", "No");

        if (answer)
        {
            await DegreePlanService.Assessments.DeletePAAssessment(PAID);
            await Navigation.PopAsync();
        }
    }

    private string AssessmentValidation()
    {
        string textPattern = @"^[a-zA-Z0-9 ]+$";
        if (string.IsNullOrWhiteSpace(newassessment.Text))
        {
            return "Please enter an PA assessment name";
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