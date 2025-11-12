using C971.Services;
using C971.Views;

namespace Degree_Plan_Mobile_App.Views;

public partial class LoginView : ContentPage
{
	private bool AccountFound = false;
    private string LoginCheck;
	public LoginView()
	{
		InitializeComponent();
	}

    public async void OnClick_Login(object sender, EventArgs e)
    {
        int accountCheck = await AccountLookup();

        if (accountCheck != 0)
        {
            await Navigation.PushAsync(new TermsView(accountCheck, LoginCheck));
            username.Text = string.Empty;
            password.Text = "";
        }
        else
        {
            await DisplayAlert("Error", "No account found with provided login information.", "ok");
        }
    }

    public async void OnClick_CreateAccount(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new CreateAccountView());
	}

    public async Task<int> AccountLookup()
    {
        var accounts = await DegreePlanService.Login.SearchForAccount();

        foreach (var account in accounts)
        {
            if (account.Username == username.Text && account.Password == password.Text)
            {
                return account.LoginId;
            }
        }
        return 0;
    }
}