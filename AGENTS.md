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
- **Scanning**: Uses `ZXing.Net.Maui`. Initialize with `.UseBarcodeReader()` in `MauiProgram.cs`.

## API
- **Base URL**: `http://localhost:3000` (dev). Prod: `https://corevent-app-fatec-d78bb2efd71a.herokuapp.com`.
- **HTTP Clients**: Typed clients via `IHttpClientFactory`:
  - `AuthApiClient` — auth endpoints (no token handler, used by `AuthTokenHandler` itself)
  - `UsersApiClient` — with `AuthTokenHandler` (auto Bearer token + refresh)
- **AuthTokenHandler** (`DelegatingHandler`): Injects `Authorization: Bearer` on every request. On 401, auto-refreshes via `POST /api/auth/refresh` and retries the original request. On refresh failure, clears tokens.

## Auth Flow
- **Storage**: `SecureStorage` with keys `access_token` and `refresh_token`. User data cached in-memory (`_cachedUser` in `AuthService`).
- **Startup**: `App.xaml.cs` shows `LoadingPage`, calls `AuthService.GetCurrentUserAsync()` (checks SecureStorage, refreshes token, fetches profile). If authenticated → `//main` tab bar. If not → `//welcome`.
- **Login**: `POST /api/auth/login` → save tokens → `GET /api/users/me` → cache user.
- **Register**: `POST /api/auth/verify-email` → user enters code → `POST /api/users` → auto-login.
- **Forgot Password**: `POST /api/auth/forgot-password` → code entry → `POST /api/auth/reset-password`.
- **Logout**: `POST /api/auth/logout` → clear tokens → navigate to `//welcome`.

## Pages & Routes

### Main TabBar (`//main`)
| Tab | Route | Page | ViewModel |
|-----|-------|------|-----------|
| Inicio | `home` | `Home` | `HomeViewModel` |
| Explorar | `explore` | `Explore` | `ExploreViewModel` |
| Ingressos | `tickets` | `Tickets` | `TicketsViewModel` |
| Perfil | `profile` | `Profile` | `ProfileViewModel` |

### Detail Pages (pushed via Shell navigation)
| Route | Page | ViewModel |
|-------|------|-----------|
| `Login` | `Login` | `LoginViewModel` |
| `Register` | `Register` | `RegisterViewModel` |
| `Welcome` | `Welcome` | `WelcomeViewModel` |
| `EmailVerification` | `EmailVerification` | `EmailVerificationViewModel` |
| `ForgotPassword` | `ForgotPassword` | `ForgotPasswordViewModel` |
| `ResetPassword` | `ResetPassword` | `ResetPasswordViewModel` |
| `EditProfile` | `EditProfile` | `EditProfileViewModel` |
| `UpdatePassword` | `UpdatePassword` | `UpdatePasswordViewModel` |
| `Privacy` | `Privacy` | `PrivacyViewModel` |
| `Settings` | `Settings` | `SettingsViewModel` |
| `Favorites` | `Favorites` | `FavoritesViewModel` |
| `PurchaseHistory` | `PurchaseHistory` | `PurchaseHistoryViewModel` |
| `Reviews` | `Reviews` | `ReviewsViewModel` |
| `PanelOrganizer` | `PanelOrganizer` | `PanelOrganizerViewModel` |
| `PanelCollaborator` | `PanelCollaborator` | `PanelCollaboratorViewModel` |
| `TransferSettings` | `TransferSettings` | `TransferSettingsViewModel` |
| `AddBankAccount` | `AddBankAccount` | `AddBankAccountViewModel` |
| `AddPixKey` | `AddPixKey` | `AddPixKeyViewModel` |
| `CreateEvent` | `CreateEvent` | `CreateEventViewModel` |
| `ManageEvent` | `ManageEvent` | `ManageEventViewModel` |
| `ParticipantList` | `ParticipantList` | `ParticipantListViewModel` |
| `EventTeam` | `EventTeam` | `EventTeamViewModel` |
| `CheckInPage` | `CheckInPage` | `CheckInViewModel` |
| `EventAttractions` | `EventAttractions` | `EventAttractionsViewModel` |
| `CollaboratorEventDetail` | `CollaboratorEventDetail` | `CollaboratorEventDetailViewModel` |
| `EventDetail` | `EventDetail` | `EventDetailViewModel` |
| `CheckoutPage` | `CheckoutPage` | `CheckoutViewModel` |

## Workflow: New Page
1. Create `Views/Page.xaml`, `Views/Page.xaml.cs`, and `ViewModels/PageViewModel.cs`.
2. Add to `CoreventApp.csproj`: `<MauiXaml Update="Views\Page.xaml"><Generator>MSBuild:Compile</Generator></MauiXaml>`.
3. Register both as `Transient` in `MauiProgram.cs`.
4. Register route in `AppShell.xaml.cs` (detail) or add to `TabBar` in `AppShell.xaml` (main tabs).

## Resources
- Colors: Use keys from `Resources/Styles/Colors.xaml` (e.g., `primary_orange_color`).
- Converters: `IntegerToVisibilityConverter` returns `bool` by comparing `int` value to `ConverterParameter` string.
