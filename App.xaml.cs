using CoreventApp.Services;
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
		Window w = new(appShell)
		{
			Width = 360,
			Height = 800,
		};

		return w;
	}

	protected override async void OnStart()
	{
		base.OnStart();

		var user = await authService.GetCurrentUserAsync();

		if (user != null)
		{
			await Shell.Current.GoToAsync("//main");
		}
	}
}