using C971.Services;
using Microsoft.VisualBasic;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Text.RegularExpressions;

namespace C971.Views;

public partial class DetailedCourseView : ContentPage
{
	private readonly int selectedCourse;
	private string OldNotes;
	public DetailedCourseView(int courseid)
	{
		InitializeComponent();

		selectedCourse = courseid;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var getCourse = await DegreePlanService.Courses.GetSelectedCourse(selectedCourse);

		foreach (var selectedcourse in getCourse)
		{
			statuspicker.SelectedItem = selectedcourse.statusPicker;
			coursename.Text = selectedcourse.courseName;
			coursestart.Date = DateTime.Parse(selectedcourse.courseStart);
			courseend.Date = DateTime.Parse(selectedcourse.courseEnd);
		}

		var getCourseDetail = await DegreePlanService.DetailedCourse.GetCourseDetails(selectedCourse);
        foreach (var detailedcourse in getCourseDetail)
		{
			statuspicker.SelectedItem = detailedcourse.statusPicker;
			coursedue.Date = detailedcourse.dueDate;
			startnotification.Date = detailedcourse.startNotification;
			endnotification.Date = detailedcourse.endNotification;
			optionalnotes.Text = detailedcourse.optionalNotes;
			OldNotes = detailedcourse.optionalNotes;
			instructorname.Text = detailedcourse.instructorName;
			instructorphone.Text = detailedcourse.instructorPhone;
			instructoremail.Text = detailedcourse.instructorEmail;
		}
	}

    public async void OnClicked_SaveCourse(object sender, EventArgs e)
    {
		if (CourseValidation() != null)
		{
			await DisplayAlert("Error", CourseValidation(), "Ok");
		}
		else
		{
			await DegreePlanService.Courses.UpdateCourses(selectedCourse, statuspicker.SelectedItem.ToString(), coursename.Text, 
				coursestart.Date, courseend.Date);

			await DegreePlanService.DetailedCourse.UpdateDetailedCourse(selectedCourse, statuspicker.SelectedItem.ToString(), 
				coursedue.Date, startnotification.Date, endnotification.Date, optionalnotes.Text, instructorname.Text,
				instructorphone.Text, instructoremail.Text);

            await DisplayAlert("Confirmation", "Course has been saved", "Ok");
        }
    }

    public async void OnClicked_DeleteCourse(object sender, EventArgs e)
	{
		bool answer = await DisplayAlert("Confirmation", "Are you sure you want to delete this course?", "Yes", "No");

		if (answer)
		{
			await DegreePlanService.Courses.DeleteCourses(selectedCourse);
			await Navigation.PopAsync();
		}
	}

    public async Task ShareText(string text)
	{
		await Share.RequestAsync(new ShareTextRequest
		{
			Text = text,
			Title = "Share Notes"
		});
	}

    public async void OnClicked_Share(object sender, EventArgs e)
    {
        if (OldNotes != optionalnotes.Text)
		{
            await DisplayAlert("Error", "Please save course details before trying to share.", "Ok");
        }
		else
		{
			if(optionalnotes.Text != null)
			{
                await ShareText(optionalnotes.Text);
            }
			else
			{
                await DisplayAlert("Error", "Please enter something into the notes text and click Save Course before trying to share.", "Ok");
            }
        }
	}

    public async void OnClicked_Assessments(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AssessmentView(selectedCourse));
	}

	void OnEditor_TextChanged(object sender, TextChangedEventArgs e)
	{
		string newText = e.NewTextValue;
	}

	public string CourseValidation()
	{
        string textPattern = @"^[a-zA-Z0-9 ]+$";
        if (string.IsNullOrWhiteSpace(coursename.Text))
		{
			return "Please enter a course name";
		}
        else if (!Regex.IsMatch(coursename.Text, textPattern))
		{
			return "Only Letters and Numbers can be used.";
		}
        else if (ValidateCourseStartAndEnd() != null)
		{
			return ValidateCourseStartAndEnd();
        }
		else if (coursedue.Date < coursestart.Date)
		{
			return "Course Due Date can not be before Course Start Date.";
		}
		else if (coursedue.Date > courseend.Date)
		{
			return "Course Due Date can not be after Course End Date.";
		}
        else if (ValidateCourseNotifications() != null)
        {
			return ValidateCourseNotifications();
        }
        else if (ValidateCourseInstructor() != null)
        {
            return ValidateCourseInstructor();
        }
        else
		{
			return null;
		}
        
	}
    public string ValidateCourseStartAndEnd()
	{
		if (coursestart.Date < DateTime.Now)
        {
            return "Course Start Date can not be in the past.";
        }
        else if (coursestart.Date == DateTime.Now)
        {
            return "Course Start Date can not start today.";
        }
        else if (courseend.Date < coursestart.Date)
        {
            return "Course End Date can not be before Course Start Date.";
        }
		else
		{
			return null;
		}
    }
    public string ValidateCourseNotifications()
	{
        if (startnotification.Date < DateTime.Now)
        {
            return "Notification Start Date can not be before todays date.";
        }
        else if (startnotification.Date == DateTime.Now)
        {
            return "Notification Start Date can not start today.";
        }
        else if (endnotification.Date < startnotification.Date)
        {
            return "End Date can not be before Start Date.";
        }
		else
		{
			return null;
		}
    }
    public string ValidateCourseInstructor()
	{
		string textPattern = @"^[a-zA-Z ]+$";
        string phonePattern = @"^\d{3}?-?\d{3}?-?\d{4}$";
        string emailPattern = @"^[a-zA-Z0-9.!@#$%^&*]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}?$";

        if (string.IsNullOrWhiteSpace(instructorname.Text))
        {
            return "Please enter your instructor name";
        }
        else if (!Regex.IsMatch(instructorname.Text, textPattern))
		{
			return "Names only consists of letters.";
		}
        else if (string.IsNullOrWhiteSpace(instructorphone.Text))
        {
            return "Please enter your instructor phone number";
        }
        else if (!Regex.IsMatch(instructorphone.Text, phonePattern))
        {
            return "Phone number format must be 111-222-3333";
        }
        else if (string.IsNullOrWhiteSpace(instructoremail.Text))
        {
            return "Please enter your instructor email";
        }
        else if (!Regex.IsMatch(instructoremail.Text, emailPattern))
        {
            return "Email format should be johndoe@email.com";
        }
		else
		{
			return null;
		}
    }
}