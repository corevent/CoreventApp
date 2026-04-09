namespace CoreventApp.Views;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    async private void Button_Clicked_Login(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Login", "Login com sucesso", "OK");
    }

    async private void LabelMissingPasswordTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Views.Register());
    }
}