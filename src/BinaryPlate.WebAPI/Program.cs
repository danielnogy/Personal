// Create a new instance of the WebApplication builder.
using BinaryPlate.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add application services to the service collection.
builder.Services.AddApplication();

// Add infrastructure services to the service collection.
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

// Add mapster configuration services to the service collection.
builder.Services.RegisterMapsterConfiguration();

// Add health checks services to the service collection.
builder.Services.AddHealthChecks();

// Add localization services to the service collection.
builder.Services.AddAppLocalization();

// Add authentication services to the service collection based on the provided configuration.
builder.Services.AddAuthentication(builder.Configuration);

// Add authorization services to the service collection.
builder.Services.AddAuthorizationBuilder();

// Add controllers services to the service collection, and configure JSON options.
builder.Services.AddControllers(options =>
{
    // Suppress implicit required attribute for non-nullable reference types.
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
})
.AddJsonOptions(options =>
{
    // Add JSON converter for enum strings and configure naming policy.
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Add validation services to the service collection, and configure validation options.
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<IApplicationDbContext>();

// Configure API behavior options.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // Suppress ModelState invalid filter.
    options.SuppressModelStateInvalidFilter = true;
});

// Configure form options.
builder.Services.Configure<FormOptions>(x =>
{
    // Set value and multipart body length limits.
    x.ValueLengthLimit = 1073741824;
    x.MultipartBodyLengthLimit = 1073741824;
});

// Add Swagger API services to the service collection.
builder.Services.AddSwaggerApi();

// Add CORS services to the service collection.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder.SetIsOriginAllowed(isOriginAllowed => true)
                                                                  .AllowAnyHeader()
                                                                  .AllowAnyMethod()
                                                                  .AllowCredentials());
});

// Add SignalR services to the service collection.
builder.Services.AddSignalR();

// Add a singleton TimerManager instance to the service collection.
builder.Services.AddSingleton<TimerManager>();

// Add scoped services for HubNotificationService and SignalRContextInfoProvider to the service collection.
builder.Services.AddScoped<IHubNotificationService, HubNotificationService>();
builder.Services.AddScoped<ISignalRContextInfoProvider, SignalRContextInfoProvider>();

// Configure Identity options.
builder.Services.Configure<IdentityOptions>(options =>
{
    // Set user account options.
    options.User.AllowedUserNameCharacters = builder.Configuration.GetValue<string>($"{AppUserOptions.Section}:AllowedUserNameCharacters");
    options.User.RequireUniqueEmail = true;

    // Set password requirements.
    options.Password.RequiredLength = builder.Configuration.GetValue<int>($"{AppPasswordOptions.Section}:RequiredLength");
    options.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>($"{AppPasswordOptions.Section}:RequiredUniqueChars");
    options.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>($"{AppPasswordOptions.Section}:RequireNonAlphanumeric");
    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>($"{AppPasswordOptions.Section}:RequireLowercase");
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>($"{AppPasswordOptions.Section}:RequireUppercase");
    options.Password.RequireDigit = builder.Configuration.GetValue<bool>($"{AppPasswordOptions.Section}:RequireDigit");

    // Set lockout options.
    options.Lockout.AllowedForNewUsers = builder.Configuration.GetValue<bool>($"{AppLockoutOptions.Section}:AllowedForNewUsers");
    options.Lockout.MaxFailedAccessAttempts = builder.Configuration.GetValue<int>($"{AppLockoutOptions.Section}:MaxFailedAccessAttempts");
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(builder.Configuration.GetValue<double>($"{AppLockoutOptions.Section}:DefaultLockoutTimeSpan"));

    // Require confirmed accounts for sign in.
    options.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>($"{AppUserOptions.Section}:RequireConfirmedAccount");
});

// Add the custom exception handler middleware to the services collection.
builder.Services.AddExceptionHandler<BpExceptionHandler>();

// Add the ProblemDetails middleware to the services collection.
builder.Services.AddProblemDetails();

// Build the app.
var app = builder.Build();

// Create a new service scope.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        // Get the tenant mode and data isolation strategy from configuration.
        var tenantMode = builder.Configuration.GetValue<TenantMode>($"{AppOptions.Section}:{AppTenantOptions.Section}:TenantMode");
        var dataIsolationStrategy = builder.Configuration.GetValue<DataIsolationStrategy>($"{AppOptions.Section}:{AppTenantOptions.Section}:DataIsolationStrategy");

        // Get the database initializer service.
        var dbInitializerService = services.GetRequiredService<IDatabaseInitializerService>();

        // Execute the appropriate database creation or initialization based on the tenant mode.
        switch (tenantMode)
        {
            // If the tenant mode is SingleTenant, create a single-tenant database.
            case TenantMode.SingleTenant:
                await dbInitializerService.CreateSingleTenantDatabase();
                break;

            // If the tenant mode is MultiTenant, initialize a multi-tenant database with the specified data isolation strategy.
            case TenantMode.MultiTenant:
                await dbInitializerService.InitializeMultiTenantDatabase(dataIsolationStrategy);
                break;
        }
    }
    catch (Exception ex)
    {
        // Log any errors that occur.
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        // Get the error string to include in the exception message.
        var errorMessage = string.IsNullOrEmpty(ex.InnerException?.ToString()) ? ex.Message : $"{ex.Message} - {ex.InnerException.Message}";

        logger.LogError(ex, $"An error occurred while migrating or seeding the database.| {errorMessage}");

        // Rethrow the exception to propagate it up the call stack.
        throw;
    }
}

// Configure health checks.
app.UseHealthChecks("/health");

// Configure HTTPS redirection.
app.UseHttpsRedirection();

// Serve static files.
app.UseStaticFiles();

// Configure CORS.
app.UseCors("CorsPolicy");

// Configure app localization.
app.UseAppLocalization();

// Configure global exception handler.
app.UseExceptionHandler();

// Configure routing.
app.UseRouting();

// Configure Swagger API.
app.UseSwaggerApi();

// Configure multi-tenancy.
app.UseMultiTenancy();

// Configure Identity options.
app.UseIdentityOptions();

// Configure authentication.
app.UseAuth();

// Add the Hangfire Dashboard UI to the middleware pipeline, allowing you to view and manage
// background jobs via a web-based dashboard If you want to access the hangfire dashboard from
// outside the localhost, please refer to this link. https://docs.hangfire.io/en/latest/configuration/using-dashboard.html.
app.UseHangfireDashboard();

// Map the default route for ASP.NET Core MVC controllers, so that incoming HTTP requests can be
// routed to the appropriate controller and action method.
app.MapControllers();

// Map the DashboardHub SignalR hub to the "Hubs/DashboardHub" URL path.
app.MapHub<DashboardHub>("Hubs/DashboardHub");

// Map the ReportingServicesHub SignalR hub to the "Hubs/ReportingServicesHub" URL path.
app.MapHub<ReportingServicesHub>("Hubs/ReportingServicesHub");

// Start the web application and listens for incoming requests.
app.Run();