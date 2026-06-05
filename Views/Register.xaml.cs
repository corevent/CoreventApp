using System.Text.RegularExpressions;

namespace CoreventApp.Views;

public partial class Register : ContentPage
{
    private bool _isUpdatingCpf;

    public Register(ViewModels.RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnCpfTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isUpdatingCpf) return;
        _isUpdatingCpf = true;

        var digits = Regex.Replace(e.NewTextValue ?? "", @"\D", "");
        if (digits.Length > 11) digits = digits[..11];

        var formatted = digits.Length switch
        {
            > 9 => $"{digits[..3]}.{digits[3..6]}.{digits[6..9]}-{digits[9..]}",
            > 6 => $"{digits[..3]}.{digits[3..6]}.{digits[6..]}",
            > 3 => $"{digits[..3]}.{digits[3..]}",
            _ => digits
        };

        if (sender is Entry entry && entry.Text != formatted)
            entry.Text = formatted;

        _isUpdatingCpf = false;
    }
}
