# Agent Guide: CoreventApp

## Architecture
- **Framework**: .NET MAUI (Multi-platform App UI) targeting .NET 10.
- **Pattern**: MVVM using `CommunityToolkit.Mvvm`.
- **Navigation**: `AppShell.xaml` defines the main structure (TabBar) and `AppShell.xaml.cs` handles route registration.
- **Dependency Injection**: Registered in `MauiProgram.cs`.

## Development Commands
- **Build**: `dotnet build`
- **Restore**: `dotnet restore`
- **Clean**: `dotnet clean`
- **Run (Windows)**: `dotnet run -f net10.0-windows10.0.19041.0` (ensure Developer Mode is enabled on Windows).
- **Run (Android)**: `dotnet build -t:Run -f net10.0-android`

## Key Locations
- **Views**: `Views/*.xaml` and their code-behinds.
- **ViewModels**: `ViewModels/*.cs` (typically `partial` classes using MVVM Toolkit source generators).
- **Models**: `Models/*.cs`.
- **Services**: `Services/*.cs`. Currently uses `MockAuthService` for authentication.
- **Resources**: `Resources/` (Images, Styles, Fonts, Splash).

## Workflow Conventions
- **New Page**:
    1. Create `MyPage.xaml` and `MyPage.xaml.cs` in `Views/`.
    2. Create `MyPageViewModel.cs` in `ViewModels/`.
    3. Register both in `MauiProgram.cs` (usually as `Transient`).
    4. Register the route in `AppShell.xaml.cs` if it's a detail page, or add to `AppShell.xaml` if it's a root tab.
    5. Add `<MauiXaml Update="Views\MyPage.xaml"><Generator>MSBuild:Compile</Generator></MauiXaml>` to `CoreventApp.csproj`.

- **MVVM Toolkit**: 
    - Use `[ObservableProperty]` for properties (requires `partial` class).
    - Use `[RelayCommand]` for methods.

## Constraints & Quirks
- **Source Generation**: `CoreventApp.csproj` uses `<MauiXamlInflator>SourceGen</MauiXamlInflator>`.
- **Nullable**: Enabled project-wide.
- **Android Target**: API 21.0 minimum.
- **Windows Target**: 10.0.17763.0 minimum.
- **Style**: Mimic `Resources/Styles/Styles.xaml` for UI consistency.
