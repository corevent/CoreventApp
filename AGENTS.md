# Agent Guide: CoreventApp

## Architecture & Commands
- **Framework**: .NET MAUI (`net10.0-android`, `net10.0-windows10.0.19041.0`).
- **Pattern**: MVVM via `CommunityToolkit.Mvvm` (`[ObservableProperty]`, `[RelayCommand]`).
- **Build**: `dotnet build`
- **Run**: `dotnet run -f net10.0-windows10.0.19041.0` (Windows) or `dotnet build -t:Run -f net10.0-android` (Android).
- **Window**: Fixed size **360x800** defined in `App.xaml.cs`.

## Critical Quirks
- **XAML Codegen**: `<MauiXamlInflator>SourceGen</MauiXamlInflator>` is enabled. Every new XAML file MUST be manually added to `CoreventApp.csproj` with `<Generator>MSBuild:Compile</Generator>` or `InitializeComponent()` will fail.
- **DI Registration**: All Views and ViewModels MUST be registered in `MauiProgram.cs` as `Transient`.
- **Partial Classes**: ViewModels and Code-behinds MUST be `partial` for source generators to work.
- **Command Naming**: `[RelayCommand]` on `DoSomethingAsync` generates `DoSomethingCommand` (strips `Async`).
- **Commented DI**: Some ViewModels are commented out in `MauiProgram.cs`. Uncomment them if implementing logic.

## New Page Workflow
1. **Files**: Create `Views/Page.xaml`, `Views/Page.xaml.cs`, and `ViewModels/PageViewModel.cs`.
2. **Csproj**: Add `<MauiXaml Update="Views\Page.xaml"><Generator>MSBuild:Compile</Generator></MauiXaml>` to `CoreventApp.csproj`.
3. **DI**: Register both in `MauiProgram.cs`.
4. **Routing**: 
   - Detail pages: `Routing.RegisterRoute` in `AppShell.xaml.cs`.
   - Main tabs: Add to `TabBar` in `AppShell.xaml`.

## Data & Auth
- **Auth**: `MockAuthService` only. Credentials: `teste@email.com` / `123456`.
- **Storage**: `SecureStorage` key `logged_user_data` stores JSON user data.
- **Resources**: Use keys from `Resources/Styles/Colors.xaml` (e.g., `primary_orange_color`).
- **Converters**: Use `IntegerToVisibilityConverter` for conditional UI.
