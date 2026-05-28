using CoreventApp.Services;
using CoreventApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace CoreventApp;

public partial class App : Application
{
	private readonly AppShell appShell;
	private readonly IAuthService authService;

	public App(AppShell appShell, IAuthService authService)
	{
		InitializeComponent();
		this.appShell = appShell;
		this.authService = authService;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		var loadingPage = new LoadingPage();

		Window w = new(loadingPage)
		{
			Title = "Corevent",
		};

		_ = InitializeAsync(w);

		return w;
	}

	private async Task InitializeAsync(Window window)
	{
		var user = await authService.GetCurrentUserAsync();

		MainThread.BeginInvokeOnMainThread(async () =>
		{
			window.Page = appShell;

			if (user != null)
			{
				await Shell.Current.GoToAsync("//main");
			}
		});
	}
}
