using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class PurchaseHistoryViewModel : ObservableObject
{
    private readonly OrdersService _ordersService;

    public PurchaseHistoryViewModel(OrdersService ordersService)
    {
        _ordersService = ordersService;
    }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsEmpty { get; set; }

    public ObservableCollection<MyOrdersDataDto> Orders { get; } = [];

    [RelayCommand]
    public async Task LoadOrders()
    {
        IsLoading = true;

        var result = await _ordersService.GetMyOrdersAsync(page: 1, limit: 50);

        Orders.Clear();
        foreach (var order in result.Data)
            Orders.Add(order);

        IsEmpty = Orders.Count == 0;
        IsLoading = false;
    }

    [RelayCommand]
    private async Task OpenOrderDetail(MyOrdersDataDto? order)
    {
        if (order is null) return;

        await Shell.Current.GoToAsync(nameof(Views.OrderDetailPage), new Dictionary<string, object>
        {
            ["OrderId"] = order.Id
        });
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
