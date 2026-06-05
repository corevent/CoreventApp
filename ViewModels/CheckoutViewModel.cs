using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;
using CoreventApp.Services.Api;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventId), "EventId")]
public partial class CheckoutViewModel : ObservableObject
{
    private readonly EventsService _eventsService;
    private readonly TicketTypesApiClient _ticketTypesApi;
    private readonly OrdersApiClient _ordersApi;
    private string? _eventId;
    private string? _orderId;
    private string? _checkoutUrl;

    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EventDate { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EventImageUrl { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsPurchasing { get; set; }

    public string? EventId
    {
        set
        {
            _eventId = value;
            if (value is not null) _ = LoadDataAsync(value);
        }
    }

    public ObservableCollection<TicketTypeDataDto> TicketTypes { get; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Subtotal))]
    [NotifyPropertyChangedFor(nameof(ServiceFee))]
    [NotifyPropertyChangedFor(nameof(Total))]
    public partial TicketTypeDataDto? SelectedTicketType { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Subtotal))]
    [NotifyPropertyChangedFor(nameof(ServiceFee))]
    [NotifyPropertyChangedFor(nameof(Total))]
    public partial int Quantity { get; set; } = 1;

    public decimal Subtotal => SelectedTicketType?.Price * Quantity ?? 0;
    public decimal ServiceFee => Subtotal * 0.10m;
    public decimal Total => Subtotal + ServiceFee;

    public CheckoutViewModel(EventsService eventsService, TicketTypesApiClient ticketTypesApi, OrdersApiClient ordersApi)
    {
        _eventsService = eventsService;
        _ticketTypesApi = ticketTypesApi;
        _ordersApi = ordersApi;
    }

    private async Task LoadDataAsync(string eventId)
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var evt = await _eventsService.GetByIdAsync(eventId);
            if (evt is null) return;

            EventName = evt.Title;
            EventDate = $"{evt.StartDate:dd MMM, yyyy • HH:mm}";
            EventImageUrl = evt.BannerUrl ?? string.Empty;

            var ticketResult = await _ticketTypesApi.GetAllAsync(eventId, availableOnly: true);
            TicketTypes.Clear();
            foreach (var tt in ticketResult.Data)
                TicketTypes.Add(tt);

            if (TicketTypes.Count > 0)
                SelectedTicketType = TicketTypes[0];
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Checkout LoadDataAsync failed: {ex.Message}");
            await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível carregar os dados do checkout.", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void SelectTicketType(TicketTypeDataDto ticketType)
    {
        SelectedTicketType = ticketType;
        Quantity = 1;
    }

    [RelayCommand]
    private void IncreaseQuantity()
    {
        if (SelectedTicketType is not null && Quantity < SelectedTicketType.AvailableQuantity)
            Quantity++;
    }

    [RelayCommand]
    private void DecreaseQuantity()
    {
        if (Quantity > 1)
            Quantity--;
    }

    [RelayCommand]
    private async Task FinalizePurchaseAsync()
    {
        if (IsPurchasing || _eventId is null || SelectedTicketType is null) return;
        IsPurchasing = true;

        try
        {
            var dto = new CreateOrderDto(new List<ItemsDto>
            {
                new(SelectedTicketType.Id, Quantity)
            });

            var result = await _ordersApi.CreateAsync(_eventId, dto);
            _orderId = result.Data.OrderId;
            _checkoutUrl = result.Data.CheckoutLinks?.FirstOrDefault()?.Href;

            if (!string.IsNullOrEmpty(_checkoutUrl))
            {
                await Browser.Default.OpenAsync(_checkoutUrl, BrowserLaunchMode.SystemPreferred);
            }

            await Shell.Current.DisplayAlertAsync("Sucesso", "Compra realizada com sucesso!", "OK");
            await Shell.Current.GoToAsync("//main/home");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Checkout FinalizePurchaseAsync failed: {ex.Message}");
            await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível finalizar a compra. Tente novamente.", "OK");
        }
        finally
        {
            IsPurchasing = false;
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
