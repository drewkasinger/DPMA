using C971.Models;
using C971.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using static C971.Services.DegreePlanService;

namespace C971.Views;

public partial class AddTermView : ContentPage
{
    private readonly int AccountID;
    public AddTermView(int accountId)
	{
		InitializeComponent();
        DateTime start = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("MM/01/yyyy"));
        newstartdate.Date = start;
        newenddate.Date = start.AddMonths(6).AddDays(-1);

        AccountID = accountId;
    }

    public async void OnClick_AddTerm(object sender, EventArgs e)
    {
        if (NewTermValidation() != null)
        {
            await DisplayAlert("Error", NewTermValidation().ToString(), "Ok");
        }
        else
        {
            await DegreePlanService.Terms.AddTerm(AccountID, newtermname.Text, newstartdate.Date.ToShortDateString(), newenddate.Date.ToShortDateString());
            await Navigation.PopAsync();
        }
    }

    public string NewTermValidation()
    {
        string textPattern = @"^[a-zA-Z0-9 ]+$";
        if (string.IsNullOrWhiteSpace(newtermname.Text))
        {
            return "Please enter a name";
        }
        else if (!Regex.IsMatch(newtermname.Text, textPattern))
        {
            return "Please only use letters and numbers, no special characters.";
        }
        else if (newstartdate.Date < DateTime.Now)
        {
            return "Start Date can not be in the past.";
        }
        else if (newstartdate.Date == DateTime.Now)
        {
            return "Start Date can not start today.";
        }
        else if (newenddate.Date < newstartdate.Date)
        {
            return "End Date can not be before Start Date.";
        }
        else
        {
            return null;
        }
    }
}