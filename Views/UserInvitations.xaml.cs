using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class UserInvitations : ContentPage
{
    public UserInvitations(UserInvitationsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserInvitationsViewModel vm)
            _ = vm.LoadInvitationsCommand.ExecuteAsync(null);
    }
}
