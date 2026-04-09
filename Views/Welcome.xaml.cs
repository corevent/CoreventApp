namespace CoreventApp.Views;

public partial class Welcome : ContentPage
{
	public Welcome()
	{
		InitializeComponent();
	}

    async private void Button_Clicked_Register(object sender, EventArgs e)
    {
		await DisplayAlertAsync("Aviso", "Crie sua conta", "Ok");
    }

    async private void Button_Clicked_Login(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Login());
    }
}