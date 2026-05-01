namespace CoreventApp.Views;

public class ProfileMenuOption
{
    public string ?Title { get; set; }
}

public partial class Profile : ContentPage
{
    public List<ProfileMenuOption> Options { get; set; }

    public Profile()
    {
        InitializeComponent();

        Options = new List<ProfileMenuOption>
        {
            new ProfileMenuOption { Title = "Dados Pessoais" },
            new ProfileMenuOption { Title = "Histórico de Compra" },
            new ProfileMenuOption { Title = "Meus Favoritos" },
            new ProfileMenuOption { Title = "Avaliações" },
            new ProfileMenuOption { Title = "Configurações" }
        };

        BindingContext = this;
    }
}
