namespace CoreventApp.Views;

public class ProfileMenuOption
{
    public string ?Title { get; set; }
    public Type? PageType { get; set; }
    public string image_path { get; set; }
}

public partial class Profile : ContentPage
{
    public List<ProfileMenuOption> Options { get; set; }

    public Profile()
    {
        InitializeComponent();

        Options = new List<ProfileMenuOption>
        {
            new ProfileMenuOption { Title = "Dados Pessoais", PageType = typeof(Views.EditProfile), image_path = "user.png"},
            new ProfileMenuOption { Title = "Histórico de Compra", PageType = typeof(Views.PurchaseHistory), image_path = "ticket.png"},
            new ProfileMenuOption { Title = "Meus Favoritos", PageType = typeof(Views.Favorites), image_path = "heart.png"},
            new ProfileMenuOption { Title = "Avaliações", PageType = typeof(Views.Reviews), image_path = "star.png"},
            new ProfileMenuOption { Title = "Configurações", PageType = typeof(Views.Settings), image_path = "settings.png"},
        };

        BindingContext = this;
    }
    async private void Button_Clicked_SairDaConta(object sender, EventArgs e)
    {
        await DisplayAlertAsync("Aviso", "Deseja mesmo encerrar a sua sessão?", "Sim", "Cancelar");
    }

    async private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selected = e.CurrentSelection.FirstOrDefault() as ProfileMenuOption;
        if (selected?.PageType == null) return;

        ((CollectionView)sender).SelectedItem = null;

        var page = (Page)Activator.CreateInstance(selected.PageType);
        await Navigation.PushAsync(page);
    }

    async private void TapGestureRecognizer_Tapped_PainelOrganizador(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Views.PanelOrganizer());
    }

    async private void TapGestureRecognizer_Tapped_PainelColaborador(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new Views.PanelCollaborator());
    }
}
