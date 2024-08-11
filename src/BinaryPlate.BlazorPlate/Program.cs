using BinaryPlate.BlazorPlate.Consumers.HttpClients;
using BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;
using BinaryPlate.BlazorPlate.Consumers.SignalRClients;
using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Popups;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXlfcXVRRWdeVkBzW0M=");
builder.RootComponents.Add<App>("#app");
builder.Services.AddSyncfusionBlazor();
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpInterceptor(builder);

builder.Services.AddAuthHttpInterceptor(builder);
builder.Services.AddTwoFactorAuthHttpInterceptor(builder);

builder.Services.AddCustomHeadersHttpInterceptor(builder);

builder.Services.AddOptions(); 

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<SnackbarApiExceptionProvider>();
builder.Services.AddScoped<UserPasswordService>();
builder.Services.AddScoped<AccessTokenProvider>();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<ReturnUrlProvider>();
builder.Services.AddScoped<UrlProvider>();

builder.Services.AddSingleton<AppStateManager>();
builder.Services.AddTransient<TimerHelper>();
builder.Services.AddSingleton<LocalizationService>();
builder.Services.AddSingleton<NavigationService>();
builder.Services.AddSingleton<BreadcrumbService>();

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<ReportingHubClient>();
builder.Services.AddScoped<DashboardHubClient>();
builder.Services.AddScoped<RefreshTokenService>();


builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IAccountsClient, AccountsClient>();
builder.Services.AddScoped<IManageClient, ManageClient>();
builder.Services.AddScoped<IRolesClient, RolesClient>();
builder.Services.AddScoped<IPermissionsClient, PermissionsClient>();
builder.Services.AddScoped<IUsersClient, UsersClient>();
builder.Services.AddScoped<IMyTenantClient, MyTenantClient>();
builder.Services.AddScoped<ITenantsClient, TenantsClient>();
builder.Services.AddScoped<IAppSettingsClient, AppSettingsClient>();
builder.Services.AddScoped<IApplicantsClient, ApplicantsClient>();
builder.Services.AddScoped<IReportsClient, ReportsClient>();
builder.Services.AddScoped<IDashboardClient, DashboardClient>();
builder.Services.AddScoped<IFileUploadClient, FileUploadClient>();
#region SSM Services
builder.Services.AddScoped<IDepartmentsClient, DepartmentsClient>();
builder.Services.AddScoped<IQuestionCategoriesClient, QuestionCategoriesClient>();
builder.Services.AddScoped<IMaterialsClient, MaterialsClient>();
builder.Services.AddScoped<IMaterialCategoriesClient, MaterialCategoriesClient>();
builder.Services.AddScoped<IQuestionsClient, QuestionsClient>();
builder.Services.AddScoped<ITestsClient, TestsClient>();
builder.Services.AddScoped<IEmployeesClient, EmployeesClient>();
builder.Services.AddScoped<SfDialogService>();
#endregion
builder.Services.AddLocalization();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.ClearAfterNavigation = false;
    config.SnackbarConfiguration.BackgroundBlurred = true;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 1000;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddAuthorizationCore();

var localStorage = builder.Build().Services.GetRequiredService<ILocalStorageService>();

var culture = await localStorage.GetItemAsync<string>("Culture");

var selectedCulture = culture == null ? new CultureInfo("en-US") : new CultureInfo(culture);

//var selectedCulture = culture; You can uncomment this line and comment the above line.

CultureInfo.DefaultThreadCurrentCulture = selectedCulture;

CultureInfo.DefaultThreadCurrentUICulture = selectedCulture;

await builder.Build().RunAsync();