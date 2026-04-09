using Microsoft.Extensions.DependencyInjection;

namespace CoreventApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
        Window w = new Window(new AppShell());
        w.Height = 600;
        w.Width = 300;

        return w;
    }
}