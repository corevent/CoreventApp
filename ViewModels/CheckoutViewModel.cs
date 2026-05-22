using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace CoreventApp.ViewModels;

public partial class CheckoutViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string EventName { get; set; } = "Tech Summit Brasil";

    [ObservableProperty]
    public partial string EventDate { get; set; } = "22 Nov, 2026 • 09:00";

    [ObservableProperty]
    public partial string EventImageUrl { get; set; } = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622?w=400&h=400&fit=crop";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Subtotal))]
    [NotifyPropertyChangedFor(nameof(ServiceFee))]
    [NotifyPropertyChangedFor(nameof(Total))]
    public partial decimal EventPrice { get; set; } = 450.00m;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Subtotal))]
    [NotifyPropertyChangedFor(nameof(ServiceFee))]
    [NotifyPropertyChangedFor(nameof(Total))]
    public partial int Quantity { get; set; } = 1;

    [ObservableProperty]
    public partial string SelectedPaymentMethod { get; set; } = "Card";

    public decimal Subtotal => EventPrice * Quantity;
    public decimal ServiceFee => Subtotal * 0.10m;
    public decimal Total => Subtotal + ServiceFee;

    [RelayCommand]
    private void IncreaseQuantity()
    {
        Quantity++;
    }

    [RelayCommand]
    private void DecreaseQuantity()
    {
        if (Quantity > 1)
            Quantity--;
    }

    [RelayCommand]
    private void SelectPaymentMethod(string method)
    {
        SelectedPaymentMethod = method;
    }

    [RelayCommand]
    private async Task FinalizePurchaseAsync()
    {
        await Shell.Current.DisplayAlertAsync("Sucesso", "Compra realizada com sucesso!", "OK");
        await Shell.Current.GoToAsync("//main/home");
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
