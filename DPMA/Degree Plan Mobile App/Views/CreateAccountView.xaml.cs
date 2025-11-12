using C971.Services;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Degree_Plan_Mobile_App.Views;

public partial class CreateAccountView : ContentPage
{
    public CreateAccountView()
	{
		InitializeComponent();
    }

	public async void OnClick_CreateAccount(object sender, EventArgs e)
	{
		var accounts = await DegreePlanService.Login.SearchForAccount();
		foreach (var account in accounts)
		{
			if (account.Username == newusername.Text && account.Password == newpassword.Text)
			{
				await DisplayAlert("Error", "Username is already taken", "Ok");
			}
			else
			{
                if (NewAccountValidation() != null)
                {
                    await DisplayAlert("Error", NewAccountValidation().ToString(), "Ok");
                }
                else
                {
                    await DegreePlanService.Login.CreateAccount(newusername.Text, newpassword.Text, newname.Text);
                    await DisplayAlert("Confirmation", "Account has been created!", "Ok");
                    await Navigation.PopAsync();
                }
            }
		}
	}

	public string NewAccountValidation()
	{
        string textPattern = @"^[a-zA-Z ]+$";
        if (string.IsNullOrWhiteSpace(newusername.Text))
        {
            return "Please enter a username";
        }
        else if (!Regex.IsMatch(newusername.Text, textPattern))
        {
            return "Please only use letters and numbers, no special characters.";
        }
        else if (string.IsNullOrWhiteSpace(newpassword.Text))
        {
            return "Please enter a password";
        }
        else if (newpassword.Text.Length < 6)
        {
            return "Password must be atleast 6 characters.";
        }
        else if (string.IsNullOrWhiteSpace(newname.Text))
        {
            return "Please enter a name";
        }
        else if (!Regex.IsMatch(newname.Text, textPattern))
        {
            return "Names only consists of letters.";
        }
        return null;
	}
}