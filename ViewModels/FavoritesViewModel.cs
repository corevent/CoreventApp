using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;

namespace CoreventApp.ViewModels;

public partial class FavoritesViewModel : ObservableObject
{
  [RelayCommand]
  private async Task DeleteFavoriteEventAsync()
  {
    bool confirm = await Shell.Current.DisplayAlertAsync("Excluir evento", "Deseja excluir este evento dos seus favoritos?", "Sim", "Não");

    if (confirm)
    {
      await Shell.Current.DisplayAlertAsync("Sucesso", "Evento excluído dos favoritos com sucesso!", "Sim");
    }
  }

  [RelayCommand]
  private async Task GoBack()
  {
      await Shell.Current.GoToAsync("..");
  }
}