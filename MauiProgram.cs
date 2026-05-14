using CoreventApp.Services;
using Microsoft.Extensions.Logging;

namespace CoreventApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("PlusJakartaSans-Regular.ttf", "PlusJakartaSansRegular");
				fonts.AddFont("PlusJakartaSans-SemiBold.ttf", "PlusJakartaSansSemiBold");
				fonts.AddFont("PlusJakartaSans-Bold.ttf", "PlusJakartaSansBold");
			});

		builder.Services.AddSingleton<AppShell>();
		
		// Services
		builder.Services.AddSingleton<IAuthService, MockAuthService>();
		
		// ViewModels
		builder.Services.AddTransient<ViewModels.WelcomeViewModel>();
		builder.Services.AddTransient<ViewModels.LoginViewModel>();
		builder.Services.AddTransient<ViewModels.RegisterViewModel>();
		builder.Services.AddTransient<ViewModels.UpdateEmailViewModel>();
		builder.Services.AddTransient<ViewModels.UpdatePasswordViewModel>();
		builder.Services.AddTransient<ViewModels.HomeViewModel>();
		// builder.Services.AddTransient<ViewModels.ExploreViewModel>();
		// builder.Services.AddTransient<ViewModels.TicketsViewModel>();
		builder.Services.AddTransient<ViewModels.ProfileViewModel>();
		builder.Services.AddTransient<ViewModels.PrivacyViewModel>();
		builder.Services.AddTransient<ViewModels.EditProfileViewModel>();
		builder.Services.AddTransient<ViewModels.FavoritesViewModel>();
		// builder.Services.AddTransient<ViewModels.PanelCollaboratorViewModel>();
		// builder.Services.AddTransient<ViewModels.PanelOrganizerViewModel>();
		// builder.Services.AddTransient<ViewModels.PurchaseHistoryViewModel>();
		// builder.Services.AddTransient<ViewModels.ReviewsViewModel>();
		builder.Services.AddTransient<ViewModels.SettingsViewModel>();

		// Views
		builder.Services.AddTransient<Views.Welcome>();
		builder.Services.AddTransient<Views.Login>();
		builder.Services.AddTransient<Views.Register>();
		builder.Services.AddTransient<Views.UpdateEmail>();
		builder.Services.AddTransient<Views.UpdatePassword>();
		builder.Services.AddTransient<Views.Home>();
		builder.Services.AddTransient<Views.Explore>();
		builder.Services.AddTransient<Views.Tickets>();
		builder.Services.AddTransient<Views.Profile>();
		builder.Services.AddTransient<Views.Privacy>();
		builder.Services.AddTransient<Views.EditProfile>();
		builder.Services.AddTransient<Views.Favorites>();
		builder.Services.AddTransient<Views.PanelCollaborator>();
		builder.Services.AddTransient<Views.PanelOrganizer>();
		builder.Services.AddTransient<Views.PurchaseHistory>();
		builder.Services.AddTransient<Views.Reviews>();
		builder.Services.AddTransient<Views.Settings>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
