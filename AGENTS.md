# Agent Guide: CoreventApp

## Architecture & Commands
- **Framework**: .NET MAUI (`net10.0-android`, `net10.0-windows10.0.19041.0`).
- **Pattern**: MVVM via `CommunityToolkit.Mvvm` (`[ObservableProperty]`, `[RelayCommand]`).
- **Build**: `dotnet build`
- **Run**: `dotnet run -f net10.0-windows10.0.19041.0` (Windows) or `dotnet build -t:Run -f net10.0-android` (Android).
- **Window**: Fixed size **360x800** in `App.xaml.cs`. Font: `PlusJakartaSansRegular`.

## Critical Quirks
- **XAML Codegen**: `<MauiXamlInflator>SourceGen</MauiXamlInflator>` is enabled. Every new XAML file **MUST** be manually added to `CoreventApp.csproj` with `<Generator>MSBuild:Compile</Generator>` or `InitializeComponent()` fails.
- **DI Registration**: All Views and ViewModels **MUST** be registered in `MauiProgram.cs` as `Transient`.
- **Partial Classes**: ViewModels and Code-behinds **MUST** be `partial` for source generators.
- **Commands**: `[RelayCommand]` on `DoSomethingAsync` generates `DoSomethingCommand` (strips `Async`).
- **Commented DI**: `ExploreViewModel` is commented out in `MauiProgram.cs`. `PurchaseHistory` and `Reviews` are currently static views with no ViewModels.
- **Scanning**: Uses `ZXing.Net.Maui`. Initialize with `.UseBarcodeReader()` in `MauiProgram.cs`.

## Workflow: New Page
1. Create `Views/Page.xaml`, `Views/Page.xaml.cs`, and `ViewModels/PageViewModel.cs`.
2. Add to `CoreventApp.csproj`: `<MauiXaml Update="Views\Page.xaml"><Generator>MSBuild:Compile</Generator></MauiXaml>`.
3. Register both as `Transient` in `MauiProgram.cs`.
4. Register route in `AppShell.xaml.cs` (detail) or add to `TabBar` in `AppShell.xaml` (main tabs).

## Data & Auth
- **Auth**: `MockAuthService` only. Credentials: `teste@email.com` / `123456`.
- **Storage**: `SecureStorage` key `logged_user_data` stores JSON user data.
- **Resources**: Use keys from `Resources/Styles/Colors.xaml` (e.g., `primary_orange_color`).
- **Converters**: `IntegerToVisibilityConverter` returns `bool` by comparing `int` value to `ConverterParameter` string.
