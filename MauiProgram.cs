using CoreventApp.Services;
using CoreventApp.Services.Api;
using MauiIcons.Cupertino;
using Microsoft.Extensions.Logging;
using ZXing.Net.Maui.Controls;

namespace CoreventApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseBarcodeReader()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("PlusJakartaSans-VariableFont_wght.ttf", "Plus Jakarta Sans");
			})
			.UseCupertinoMauiIcons();

		builder.Services.AddSingleton<AppShell>();

		// Services
		builder.Services.AddSingleton<TokenService>();
		builder.Services.AddTransient<AuthTokenHandler>();

		string baseUrl = "https://corevent-app-fatec-d78bb2efd71a.herokuapp.com/";
		builder.Services.AddHttpClient<AuthApiClient>(c => c.BaseAddress = new Uri(baseUrl));
		builder.Services.AddHttpClient<UsersApiClient>(c => c.BaseAddress = new Uri(baseUrl))
			.AddHttpMessageHandler<AuthTokenHandler>();

		builder.Services.AddHttpClient<PaymentInfoApiClient>(c => c.BaseAddress = new Uri(baseUrl))
			.AddHttpMessageHandler<AuthTokenHandler>();

		builder.Services.AddHttpClient<StatesApiClient>(c => c.BaseAddress = new Uri(baseUrl))
			.AddHttpMessageHandler<AuthTokenHandler>();

		builder.Services.AddHttpClient<EventsApiClient>(c => c.BaseAddress = new Uri(baseUrl))
			.AddHttpMessageHandler<AuthTokenHandler>();

		builder.Services.AddHttpClient<AttractionsApiClient>(c => c.BaseAddress = new Uri(baseUrl))
			.AddHttpMessageHandler<AuthTokenHandler>();

		builder.Services.AddSingleton<IAuthService, AuthService>();
		builder.Services.AddTransient<PaymentInfoService>();
		builder.Services.AddTransient<EventsService>();
		builder.Services.AddTransient<AttractionsService>();

		// ViewModels
		builder.Services.AddTransient<ViewModels.WelcomeViewModel>();
		builder.Services.AddTransient<ViewModels.LoginViewModel>();
		builder.Services.AddTransient<ViewModels.RegisterViewModel>();
		builder.Services.AddTransient<ViewModels.UpdateEmailViewModel>();
		builder.Services.AddTransient<ViewModels.UpdatePasswordViewModel>();
		builder.Services.AddTransient<ViewModels.HomeViewModel>();
		builder.Services.AddTransient<ViewModels.ExploreViewModel>();
		builder.Services.AddTransient<ViewModels.TicketsViewModel>();
		builder.Services.AddTransient<ViewModels.ProfileViewModel>();
		builder.Services.AddTransient<ViewModels.PrivacyViewModel>();
		builder.Services.AddTransient<ViewModels.EditProfileViewModel>();
		builder.Services.AddTransient<ViewModels.FavoritesViewModel>();
		builder.Services.AddTransient<ViewModels.PanelCollaboratorViewModel>();
		builder.Services.AddTransient<ViewModels.PanelOrganizerViewModel>();
		builder.Services.AddTransient<ViewModels.TransferSettingsViewModel>();
		builder.Services.AddTransient<ViewModels.EmailVerificationViewModel>();
		builder.Services.AddTransient<ViewModels.ForgotPasswordViewModel>();
		builder.Services.AddTransient<ViewModels.ResetPasswordViewModel>();
		builder.Services.AddTransient<ViewModels.AddBankAccountViewModel>();
		builder.Services.AddTransient<ViewModels.AddPixKeyViewModel>();
		builder.Services.AddTransient<ViewModels.PurchaseHistoryViewModel>();
		builder.Services.AddTransient<ViewModels.ReviewsViewModel>();
		builder.Services.AddTransient<ViewModels.CreateEventViewModel>();
		builder.Services.AddTransient<ViewModels.ManageEventViewModel>();
		builder.Services.AddTransient<ViewModels.ParticipantListViewModel>();
		builder.Services.AddTransient<ViewModels.EventTeamViewModel>();
		builder.Services.AddTransient<ViewModels.CheckInViewModel>();
		builder.Services.AddTransient<ViewModels.EventAttractionsViewModel>();
		builder.Services.AddTransient<ViewModels.SettingsViewModel>();
		builder.Services.AddTransient<ViewModels.CollaboratorEventDetailViewModel>();
		builder.Services.AddTransient<ViewModels.EventDetailViewModel>();
		builder.Services.AddTransient<ViewModels.CheckoutViewModel>();

		// Views
		builder.Services.AddTransient<Views.CreateEvent>();
		builder.Services.AddTransient<Views.ManageEvent>();
		builder.Services.AddTransient<Views.ParticipantList>();
		builder.Services.AddTransient<Views.EventTeam>();
		builder.Services.AddTransient<Views.CheckInPage>();
		builder.Services.AddTransient<Views.EventAttractions>();
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
		builder.Services.AddTransient<Views.TransferSettings>();
		builder.Services.AddTransient<Views.AddBankAccount>();
		builder.Services.AddTransient<Views.AddPixKey>();
		builder.Services.AddTransient<Views.PurchaseHistory>();
		builder.Services.AddTransient<Views.EmailVerification>();
		builder.Services.AddTransient<Views.ForgotPassword>();
		builder.Services.AddTransient<Views.ResetPassword>();
		builder.Services.AddTransient<Views.Reviews>();
		builder.Services.AddTransient<Views.Settings>();
		builder.Services.AddTransient<Views.CollaboratorEventDetail>();
		builder.Services.AddTransient<Views.EventDetail>();
		builder.Services.AddTransient<Views.CheckoutPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
