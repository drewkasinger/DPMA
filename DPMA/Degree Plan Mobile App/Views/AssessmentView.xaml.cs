using C971.Models;
using C971.Services;
using System.Runtime.CompilerServices;
using static C971.Services.DegreePlanService;

namespace C971.Views;

public partial class AssessmentView : ContentPage
{
    private int OAID;
    private int PAID;
    private string OAName;
    private string PAName;
    private readonly int CourseId;
	public AssessmentView(int courseid)
	{
		InitializeComponent();

        CourseId = courseid;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var getoaassessment = await DegreePlanService.Assessments.GetOAAssessment(CourseId);
        if (getoaassessment.Count() != 0)
        {
            foreach (var oaassessment in getoaassessment)
            {
                OAID = oaassessment.OAID;
                newoa.Text = oaassessment.OA;
            }
        }
        else
        {
            newoa.Text = "No OA Assessment has been added";
        }

        var getpaassessment = await DegreePlanService.Assessments.GetPAAssessment(CourseId);
        if(getpaassessment.Count() != 0)
        {
            foreach (var paassessment in getpaassessment)
            {
                PAID = paassessment.PAID;
                newpa.Text = paassessment.PA;
            }
        }
        else
        {
            newpa.Text = "No PA Assessment has been added";
        }
    }

    public async void OnClick_EditOA(object sender, EventArgs e)
    {
        OAName = newoa.Text;
        await Navigation.PushAsync(new EditOAView(CourseId, OAID, OAName));
    }

    public async void OnClick_EditPA(object sender, EventArgs e)
    {
        PAName = newpa.Text;
        await Navigation.PushAsync(new EditPAView(CourseId, PAID, PAName));
    }
}