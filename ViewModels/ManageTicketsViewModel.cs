using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services.Api;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventId), "EventId")]
public partial class ManageTicketsViewModel : ObservableObject
{
    private readonly TicketTypesApiClient _ticketTypesApi;

    [ObservableProperty]
    public partial string EventId { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewPrice { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewTotalQuantity { get; set; } = string.Empty;

    [ObservableProperty]
    public partial DateTime NewStartDate { get; set; } = DateTime.Today;

    [ObservableProperty]
    public partial DateTime NewEndDate { get; set; } = DateTime.Today.AddMonths(1);

    [ObservableProperty]
    public partial bool HasTickets { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public ObservableCollection<TicketTypeViewModel> TicketTypes { get; } = new();

    public ManageTicketsViewModel(TicketTypesApiClient ticketTypesApi)
    {
        _ticketTypesApi = ticketTypesApi;
    }

    partial void OnEventIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
            _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        if (IsLoading || string.IsNullOrEmpty(EventId)) return;
        IsLoading = true;

        try
        {
            var result = await _ticketTypesApi.GetAllAsync(EventId, page: 1, limit: 100, availableOnly: false);
            TicketTypes.Clear();
            foreach (var tt in result.Data)
                TicketTypes.Add(MapToPresentation(tt));
            HasTickets = TicketTypes.Count > 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"ManageTickets LoadDataAsync failed: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task AddTicketTypeAsync()
    {
        if (string.IsNullOrWhiteSpace(NewName) ||
            string.IsNullOrWhiteSpace(NewPrice) ||
            string.IsNullOrWhiteSpace(NewTotalQuantity) ||
            string.IsNullOrEmpty(EventId))
            return;

        if (!decimal.TryParse(NewPrice, out var price) ||
            !int.TryParse(NewTotalQuantity, out var quantity))
            return;

        var dto = new CreateTicketTypeDto(
            NewName.Trim(),
            price,
            quantity,
            NewStartDate,
            NewEndDate);

        try
        {
            var result = await _ticketTypesApi.CreateAsync(EventId, dto);
            if (result is null) return;

            TicketTypes.Add(MapToPresentation(result.Data));
            HasTickets = true;

            NewName = string.Empty;
            NewPrice = string.Empty;
            NewTotalQuantity = string.Empty;
            NewStartDate = DateTime.Today;
            NewEndDate = DateTime.Today.AddMonths(1);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"ManageTickets AddTicketTypeAsync failed: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task DeleteTicketTypeAsync(TicketTypeViewModel ticketType)
    {
        try
        {
            await _ticketTypesApi.DeleteAsync(ticketType.Id);
            TicketTypes.Remove(ticketType);
            HasTickets = TicketTypes.Count > 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"ManageTickets DeleteTicketTypeAsync failed: {ex.Message}", "OK");
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private static TicketTypeViewModel MapToPresentation(TicketTypeDataDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Price = dto.Price,
        TotalQuantity = dto.TotalQuantity,
        AvailableQuantity = dto.AvailableQuantity
    };
}

public partial class TicketTypeViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Id { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial decimal Price { get; set; }

    [ObservableProperty]
    public partial int TotalQuantity { get; set; }

    [ObservableProperty]
    public partial int AvailableQuantity { get; set; }

    public string FormattedPrice => $"R$ {Price:F2}";
    public string AvailableLabel => $"{AvailableQuantity} / {TotalQuantity} disponíveis";
}
