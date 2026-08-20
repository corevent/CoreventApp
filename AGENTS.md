# Agent Guide: CoreventApp

## Architecture & Commands
- **Framework**: .NET MAUI (`net10.0-android`, `net10.0-windows10.0.19041.0`). `WindowsPackageType=None` (unpackaged) except Windows `Release` builds (MSIX).
- **Pattern**: MVVM via `CommunityToolkit.Mvvm` source generators (`[ObservableProperty]`, `[RelayCommand]`).
- **Build**: `dotnet build`
- **Run**: `dotnet run -f net10.0-windows10.0.19041.0` (Windows) or `dotnet build -t:Run -f net10.0-android` (Android).
- **Font**: registered as `"Plus Jakarta Sans"` (with spaces) from `PlusJakartaSans-VariableFont_wght.ttf` in `MauiProgram.cs`; referenced as `FontFamily="Plus Jakarta Sans"` in `Resources/Styles/Styles.xaml`.
- **Icons**: `AathifMahir.Maui.MauiIcons.Cupertino` package, initialized via `.UseCupertinoMauiIcons()`.

## Critical Quirks
- **XAML Codegen**: `<MauiXamlInflator>SourceGen</MauiXamlInflator>` is enabled. Every new XAML file **MUST** be manually added to `CoreventApp.csproj` as `<MauiXaml Update="Views\X.xaml"><Generator>MSBuild:Compile</Generator></MauiXaml>` or `InitializeComponent()` fails.
- **DI Registration**: All Views and ViewModels **MUST** be registered in `MauiProgram.cs`. ViewModels as `Transient`; Views as `Transient`; `AppShell` and `TokenService` are `Singleton`.
- **Partial Classes**: ViewModels and Code-behinds **MUST** be `partial` for source generators.
- **Commands**: `[RelayCommand]` on `DoSomethingAsync` generates `DoSomethingCommand` (strips `Async`).
- **Scanning**: Uses `ZXing.Net.Maui` (`.UseBarcodeReader()`) + `QRCoder` for ticket QR generation.
- **JSON**: All API serialization uses `JsonConfig.Options` (camelCase, `UtcDateTimeConverter` in `Services/Api/JsonConfig.cs`).

## API
- **Base URL**: hardcoded in `MauiProgram.cs` to `https://corevent-app-fatec-d78bb2efd71a.herokuapp.com/`. There is no dev/localhost URL — point at a local API by editing `MauiProgram.cs` (both the client and the app run against the same URL).
- **HTTP Clients**: Typed clients registered via `IHttpClientFactory` in `MauiProgram.cs`:
  - `AuthApiClient` — NO token handler (used by `AuthTokenHandler` itself to avoid recursion).
  - All other `*ApiClient` classes (Users, Events, Orders, Tickets, CheckIn, Favorites, etc.) get `.AddHttpMessageHandler<AuthTokenHandler>()` for auto Bearer token + refresh.
- **AuthTokenHandler** (`DelegatingHandler`): Injects `Authorization: Bearer`. On 401, refreshes via `POST /api/auth/refresh`, saves new tokens, retries once. On refresh failure, clears tokens.

## Auth Flow
- **Storage**: `SecureStorage` keys `access_token` / `refresh_token` (`TokenService`). User data cached in-memory (`_cachedUser` in `AuthService`).
- **Startup**: `App.xaml.cs` opens a raw `LoadingPage` window, calls `AuthService.GetCurrentUserAsync()` (refresh token + fetch profile), then swaps `window.Page = appShell`. Authenticated → `//main`; else stay on `//welcome`.
- **Login**: `POST /api/auth/login` → save tokens → `GET /api/users/me` → cache user.
- **Register**: `POST /api/auth/verify-email` (sends code) → user enters code → `POST /api/auth/register` with `VerifyEmailCode` in `RegisterDto` → auto-login.
- **Forgot Password**: `POST /api/auth/forgot-password` → code entry → `POST /api/auth/reset-password`.
- **Logout**: `POST /api/auth/logout` → clear tokens.

## Navigation & Deep Links
- **TabBar** route is `//main` (tabs: `home`, `explore`, `tickets`, `profile`). `welcome` is a top-level `ShellContent` outside the TabBar. Detail pages are registered via `Routing.RegisterRoute` in `AppShell.xaml.cs`.
- **Deep links** (`corevent://` scheme, handled in `App.xaml.cs`): `corevent://orders` → `//main/tickets`; `corevent://invites` → `UserInvitations`. Deep links only work on Windows when packaged/installed as MSIX (see README for cert + `dotnet publish` steps) — `dotnet run` does not register the protocol.

## Routes
Detail pages (pushed via Shell navigation). Route name = page class name:
`Login`, `Register`, `UpdatePassword`, `Privacy`, `EditProfile`, `PurchaseHistory`, `Favorites`, `Reviews`, `Settings`, `PanelOrganizer`, `TransferSettings`, `AddBankAccount`, `AddPixKey`, `PanelCollaborator`, `CreateEvent`, `ManageEvent`, `ParticipantList`, `EventTeam`, `CheckInPage`, `EventAttractions`, `CollaboratorEventDetail`, `EventDetail`, `ManageTicketsPage`, `CheckoutPage`, `EmailVerification`, `ForgotPassword`, `ResetPassword`, `UserInvitations`, `TicketQrCodePage`, `OrderDetailPage`.

Each `Views/Page.xaml` has a matching `ViewModels/PageViewModel.cs` (e.g. `CheckInPage` → `CheckInViewModel`, `ManageTicketsPage` → `ManageTicketsViewModel`).

## Workflow: New Page
1. Create `Views/Page.xaml`, `Views/Page.xaml.cs`, and `ViewModels/PageViewModel.cs`.
2. Add `<MauiXaml Update="Views\Page.xaml"><Generator>MSBuild:Compile</Generator></MauiXaml>` to `CoreventApp.csproj`.
3. Register View + ViewModel as `Transient` in `MauiProgram.cs`.
4. Register route in `AppShell.xaml.cs` (detail) or add to `TabBar` in `AppShell.xaml` (main tabs).

## Resources
- Colors: Use keys from `Resources/Styles/Colors.xaml` (e.g., `primary_orange_color`).
- Converters in `Converters/`: `IntegerToVisibilityConverter` returns `bool` by comparing the `int` value to the `ConverterParameter` string; others: `InvertedBoolConverter`, `NotNullToVisibilityConverter`, `RatingToStarsConverter`, `CategoryDisplayConverter`, `LocationTypeDisplayConverter`, `StatusDisplayConverter`.