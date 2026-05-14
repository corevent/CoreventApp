using CoreventApp.Views;

namespace CoreventApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(Login), typeof(Login));
		Routing.RegisterRoute(nameof(Register), typeof(Register));
		Routing.RegisterRoute(nameof(Privacy), typeof(Privacy));
		Routing.RegisterRoute(nameof(EditProfile), typeof(EditProfile));
		Routing.RegisterRoute(nameof(PurchaseHistory), typeof(PurchaseHistory));
		Routing.RegisterRoute(nameof(Favorites), typeof(Favorites));
		Routing.RegisterRoute(nameof(Reviews), typeof(Reviews));
		Routing.RegisterRoute(nameof(Settings), typeof(Settings));
		Routing.RegisterRoute(nameof(PanelOrganizer), typeof(PanelOrganizer));
		Routing.RegisterRoute(nameof(PanelCollaborator), typeof(PanelCollaborator));
	}
}
