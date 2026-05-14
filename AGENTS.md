# Agent Guide: CoreventApp

## Architecture
- **Framework**: .NET MAUI targeting `net10.0-android` / `net10.0-windows10.0.19041.0`.
- **Pattern**: MVVM via `CommunityToolkit.Mvvm` 8.4.2 (`[ObservableProperty]`, `[RelayCommand]`, `partial` classes).
- **DI**: All services, ViewModels, and Views registered in `MauiProgram.cs`.
- **Navigation**: `AppShell.xaml` defines `TabBar` (Home, Explore, Tickets, Profile) + standalone Welcome. `AppShell.xaml.cs` registers detail-push routes via `Routing.RegisterRoute`.

## Key Commands
- **Build**: `dotnet build`
- **Run Windows**: `dotnet run -f net10.0-windows10.0.19041.0` (needs Windows Developer Mode)
- **Run Android**: `dotnet build -t:Run -f net10.0-android`

## DI Quirks
- Several ViewModels are **commented out** in DI registration (`ExploreViewModel`, `PanelCollaboratorViewModel`, `PanelOrganizerViewModel`, `PurchaseHistoryViewModel`, `ReviewsViewModel`) but their **View pages are still registered** and referenced in AppShell. Adding them back requires uncommenting in `MauiProgram.cs` and optionally in `AppShell.xaml.cs`.

## New Page Workflow
1. Create `Views/MyPage.xaml` + `Views/MyPage.xaml.cs`.
2. Create `ViewModels/MyPageViewModel.cs`.
3. Register both as `Transient` in `MauiProgram.cs`.
4. Register route in `AppShell.xaml.cs` (detail page) or add to `AppShell.xaml` TabBar.
5. Add `<MauiXaml Update="Views\MyPage.xaml"><Generator>MSBuild:Compile</Generator></MauiXaml>` to `CoreventApp.csproj` (required because `<MauiXamlInflator>SourceGen</MauiXamlInflator>` is set project-wide).

## Auth
- Only `MockAuthService` exists (no real backend). Test credentials: `teste@email.com` / `123456`.
- Session persisted via `SecureStorage` (key: `logged_user_data`).

## Style / Quirks
- **Fonts**: OpenSans-Regular/Semibold + PlusJakartaSans-Regular/SemiBold/Bold. Both registered in `MauiProgram.cs`.
- **Window**: Fixed size 360×800 in `App.xaml.cs:CreateWindow`.
- **Colors**: Custom keys in `Resources/Styles/Colors.xaml` (e.g. `background_color`, `primary_orange_color`, `gradient_button`).
- **Nullable**: Enabled project-wide.
- **Android min**: API 21 / **Windows min**: 10.0.17763.0.
- **Converter**: `IntegerToVisibilityConverter` in `Converters/`.
- No tests or CI pipeline exist.
