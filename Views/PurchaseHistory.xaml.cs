namespace CoreventApp.Views;

public partial class PurchaseHistory : ContentPage
{
	public PurchaseHistory(ViewModels.PurchaseHistoryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}