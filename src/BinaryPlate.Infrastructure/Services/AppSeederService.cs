namespace BinaryPlate.Infrastructure.Services;

public class AppSeederService : IAppSeederService
{
    #region Private Fields

    private const string Admin = "Admin";
    private const string ReadOnlyOfficerRole = "Read-Only-Officer";
    private const string FullPrivilegedOfficerRole = "Full-Privileged-Officer";

    private readonly ApplicationUserManager _userManager;
    private readonly ApplicationRoleManager _roleManager;
    private readonly IApplicationDbContext _dbContext;
    private readonly IPermissionScanner _permissionScanner;
    private readonly IOptions<IdentityOptions> _identityOptions;

    #endregion Private Fields

    #region Public Constructors

    public AppSeederService(ApplicationUserManager userManager,
                            ApplicationRoleManager roleManager,
                            IApplicationDbContext dbContext,
                            IPermissionScanner permissionScanner,
                            IOptions<IdentityOptions> identityOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _identityOptions = identityOptions;
        _permissionScanner = permissionScanner;
        DisablePasswordComplexity();
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<bool> SeedTenantWithSharedDatabaseStrategy()
    {
        // Return the result of seeding the tenant.
        return await SeedTenantDatabase();
    }

    public async Task<bool> SeedTenantWithSeparateDatabaseStrategy()
    {
        // Scan and seed the built-in permissions.
        await _permissionScanner.InitializeDefaultPermissions();
        // Return the result of seeding the tenant.
        return await SeedTenantDatabase();
    }

    public async Task<bool> SeedSingleTenantModeDatabase()
    {
        // Scan and seed the built-in permissions.
        await _permissionScanner.InitializeDefaultPermissions();

        // Create a new admin user.
        var adminUser = new ApplicationUser
        {
            Email = "admin@demo",
            UserName = "admin@demo",
            Name = "Marcella",
            Surname = "Wallace",
            JobTitle = "Administrator",
            EmailConfirmed = true,
            IsStatic = true,
            IsSuperAdmin = true
        };

        // Create a new admin role.
        var adminRole = new ApplicationRole
        {
            Name = "Admin",
            IsStatic = true
        };

        // Grant permissions to the admin role for single-tenant applications.
        await GrantPermissionsForSingleTenantModeAdminRole(adminRole);

        // Create the admin role using the RoleManager.
        var adminRoleResult = await _roleManager.CreateAsync(adminRole);

        if (!adminRoleResult.Succeeded)
            // Return the admin role result if it was not successful.
            return false;

        // Create the admin user using the UserManager.
        var adminUserResult = await _userManager.CreateAsync(adminUser, "123456");

        if (!adminUserResult.Succeeded)
            // Return the admin user result if it was not successful.
            return false;

        // Add the admin user to the admin role using the UserManager.
        var adminAddToRoleResult = await _userManager.AddToRoleAsync(adminUser, adminRole.Name);

        // Return the result indicating whether the admin user was successfully added to the admin role.
        return adminAddToRoleResult.Succeeded;
    }

    public async Task<bool> SeedHostDatabase()
    {
        // Scan and seed the built-in permissions.
        await _permissionScanner.InitializeDefaultPermissions();

        // Seed static roles for the host application and check if successful.
        var seedStaticRolesForHostAppResultSucceeded = await SeedStaticHostRoles();

        if (!seedStaticRolesForHostAppResultSucceeded)
            // Return false if the seeding of static roles for the host application fails.
            return false;

        // Seed the super admin user for the host application and get the result.
        var superAdminHostAppResultSucceeded = await SeedSuperAdmin();

        // Return the status value indicating whether the super admin user was seeded successfully.
        return superAdminHostAppResultSucceeded;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Seeds the tenant by performing various data seeding operations.
    /// </summary>
    /// <returns>A boolean value indicating whether the seeding was successful.</returns>
    private async Task<bool> SeedTenantDatabase()
    {
        // Seed demo applicants
        await SeedDemoApplicants();

        // Seed static roles for the tenant and assign the result to the 'success' variable
        var seedStaticRolesResultSucceeded = await SeedStaticTenantRoles();

        // If seeding static roles fails, return false
        if (!seedStaticRolesResultSucceeded)
            return false;

        // Seed the super admin for the tenant and assign the result to the
        // 'superAdminResultSucceeded' variable
        var seedSuperAdminResultSucceeded = await SeedSuperAdmin();

        // If seeding the super admin fails, return false
        if (!seedSuperAdminResultSucceeded)
            return false;

        // Seed demo officer users and assign the result to the 'demoOfficersUsersResultSucceeded' variable
        var seedDemoOfficersUsersResultSucceeded = await SeedDemoOfficersUsers();

        // If seeding demo officer users fails, return false
        if (!seedDemoOfficersUsersResultSucceeded)
            return false;

        // If seeding succeeds, return true
        return true;
    }

    /// <summary>
    /// Seeds the super admin user for the host.
    /// </summary>
    /// <returns>A boolean value indicating whether the seeding was successful.</returns>
    private async Task<bool> SeedSuperAdmin()
    {
        // Create an admin user with demo email and password
        var adminUser = new ApplicationUser
        {
            Email = "admin@demo",
            UserName = "admin@demo",
            Name = "Marcella",
            Surname = "Wallace",
            JobTitle = "Administrator",
            EmailConfirmed = true,
            IsStatic = true,
            IsSuperAdmin = true
        };

        // Create the admin user
        var result = await _userManager.CreateAsync(adminUser, "123456");

        // Return false if admin user creation fails
        if (!result.Succeeded)
            return false;

        // Add the admin user to the Admin role
        result = await _userManager.AddToRoleAsync(adminUser, Admin);

        return result.Succeeded;
    }

    /// <summary>
    /// Seeds the static roles for the tenant.
    /// </summary>
    /// <returns>A boolean value indicating whether the static roles were seeded successfully.</returns>
    private async Task<bool> SeedStaticTenantRoles()
    {
        // Create ApplicationRoles for each static role.
        var rolesToCreate = new List<ApplicationRole>
        {
            new() { Name = "Admin", IsStatic = true },
            new() { Name = "User", IsStatic = false },
            new() { Name = "Auditor", IsStatic = false },
            new() { Name = "Accountant", IsStatic = false },
            new() { Name = "CEO", IsStatic = false }
        };

        // Use RoleManager to create each role.
        var adminRoleResult = await _roleManager.CreateAsync(rolesToCreate.First(r => r.Name == "Admin"));
        var userRoleResult = await _roleManager.CreateAsync(rolesToCreate.First(r => r.Name == "User"));
        var auditorRoleResult = await _roleManager.CreateAsync(rolesToCreate.First(r => r.Name == "Auditor"));
        var accountantRoleResult = await _roleManager.CreateAsync(rolesToCreate.First(r => r.Name == "Accountant"));
        var ceoRoleResult = await _roleManager.CreateAsync(rolesToCreate.First(r => r.Name == "CEO"));

        // Check if all roles were created successfully and return the result.
        return adminRoleResult.Succeeded &&
               userRoleResult.Succeeded &&
               auditorRoleResult.Succeeded &&
               accountantRoleResult.Succeeded &&
               ceoRoleResult.Succeeded;
    }

    /// <summary>
    /// Seeds the static roles for the host application.
    /// </summary>
    /// <returns>A boolean value indicating whether the static roles were seeded successfully.</returns>
    private async Task<bool> SeedStaticHostRoles()
    {
        // Create ApplicationRoles for each static role.
        var adminRole = new ApplicationRole
        {
            Name = "Admin",
            IsStatic = true
        };

        // Grant permissions to the Admin role for the host application.
        await GrantPermissionsForHostAppAdminRole(adminRole);

        // Use RoleManager to create the Admin role.
        var adminRoleResult = await _roleManager.CreateAsync(adminRole);

        // Check if the Admin role was created successfully and return the result.
        return adminRoleResult.Succeeded;
    }

    /// <summary>
    /// Seeds the demo officer users and their associated roles.
    /// </summary>
    /// <returns>A boolean value indicating whether the seeding of demo officer users was successful.</returns>
    private async Task<bool> SeedDemoOfficersUsers()
    {
        // Create the Full-Privileged-Officer role.
        var fullPrivilegedOfficerRole = new ApplicationRole
        {
            Name = FullPrivilegedOfficerRole,
        };

        // Create the Read-Only-Officer role.
        var readOnlyOfficerRole = new ApplicationRole
        {
            Name = ReadOnlyOfficerRole,
        };

        // Grant permissions to the Full-Privileged-Officer role.
        await GrantPermissionsForFullPrivilegedOfficerRole(fullPrivilegedOfficerRole);

        // Grant permissions to the Read-Only-Officer role.
        await GrantPermissionsForReadOnlyOfficer(readOnlyOfficerRole);

        // Create the Full-Privileged-Officer role if it doesn't exist.
        if (!await _roleManager.RoleExistsAsync(fullPrivilegedOfficerRole.Name))
        {
            var fullPrivilegedOfficerRoleResult = await _roleManager.CreateAsync(fullPrivilegedOfficerRole);

            // Return false if the Full-Privileged-Officer role creation fails.
            if (!fullPrivilegedOfficerRoleResult.Succeeded)
                return false;
        }

        // Create the Read-Only-Officer role if it doesn't exist.
        if (!await _roleManager.RoleExistsAsync(readOnlyOfficerRole.Name))
        {
            var readOnlyOfficerRoleResult = await _roleManager.CreateAsync(readOnlyOfficerRole);

            // Return false if the Read-Only-Officer role creation fails.
            if (!readOnlyOfficerRoleResult.Succeeded)
                return false;
        }

        // Create the officer users.
        var createOfficersUsersResultSucceeded = await CreateOfficersUsers();

        // Return the result of creating demo officer users and assigning roles.
        return createOfficersUsersResultSucceeded;
    }

    /// <summary>
    /// Creates the demo users.
    /// </summary>
    /// <returns>A boolean value indicating whether the creation of demo officer users was successful.</returns>
    private async Task<bool> CreateOfficersUsers()
    {
        // Create a full privileged officer user.
        var fullPrivilegedOfficer = new ApplicationUser
        {
            Email = "john@demo",
            UserName = "john@demo",
            Name = "John",
            Surname = "Smith",
            JobTitle = "Full Privileged Officer",
            EmailConfirmed = true,
        };

        // Create full privileged officer user.
        var fullPrivilegedOfficerResult = await _userManager.CreateAsync(fullPrivilegedOfficer, "123456");

        // If full privileged officer creation failed, return failure result.
        if (!fullPrivilegedOfficerResult.Succeeded)
            return false;

        // Add the full privileged officer user to the full privileged officer role.
        var fullPrivilegedOfficerAddToRoleResult = await _userManager.AddToRoleAsync(fullPrivilegedOfficer, FullPrivilegedOfficerRole);

        // If adding the full privileged officer to the role failed, return failure result.
        if (!fullPrivilegedOfficerAddToRoleResult.Succeeded)
            return false;

        // Create a read-only officer user.
        var readOnlyOfficer = new ApplicationUser
        {
            Email = "mandy@demo",
            UserName = "mandy@demo",
            Name = "Mandy",
            Surname = "Moore",
            JobTitle = "Read Only Officer",
            EmailConfirmed = true,
        };

        // Create the read-only officer user.
        var readOnlyOfficerResult = await _userManager.CreateAsync(readOnlyOfficer, "123456");

        // If creating the read-only officer user failed, return failure result.
        if (!readOnlyOfficerResult.Succeeded)
            return false;

        // Add the read-only officer user to the read-only officer role.
        var readOnlyOfficerAddToRoleResult = await _userManager.AddToRoleAsync(readOnlyOfficer, ReadOnlyOfficerRole);

        // Return the result of adding the read-only officer user to the read-only officer role.
        return readOnlyOfficerAddToRoleResult.Succeeded;
    }

    /// <summary>
    /// Seeds the demo applicants.
    /// </summary>
    private async Task SeedDemoApplicants()
    {
        // Loop through 11 times to create 11 demo applicants.
        for (var a = 0; a < 11; a++)
        {
            // Create a collection of 15 random references for each applicant.
            var referencesList = new Collection<Reference>();
            for (var r = 0; r < 6; r++)
            {
                // Create a new reference with random values for each field.
                var fakeReferences = new Faker<Reference>()
                    .RuleFor(fr => fr.Name, f => $"{f.Person.FirstName} {f.Person.LastName}")
                    .RuleFor(fr => fr.JobTitle, f => f.Name.JobTitle())
                    .RuleFor(fr => fr.Phone, f => f.Phone.PhoneNumberFormat())
                    .RuleFor(fr => fr.Address, f => f.Address.FullAddress());

                // Create a reference object from the generated values.
                Reference reference = fakeReferences;

                // Add the reference to the collection.
                referencesList.Add(reference);
            }

            // Calculate the minimum and maximum date of birth to be between 18 and 28 years old.
            var maxBirthDate = DateTime.Today.AddYears(-18);
            var minBirthDate = maxBirthDate.AddYears(-10);

            // Create a new applicant with random values for each field, between the ages of 18 and 28.
            var fakeApplicants = new Faker<Applicant>()
                .RuleFor(fa => fa.Ssn, f => f.Random.Number(123654789, 987654321))
                .RuleFor(fa => fa.FirstName, f => f.Person.FirstName)
                .RuleFor(fa => fa.LastName, f => f.Person.LastName)
                .RuleFor(fa => fa.DateOfBirth, f => f.Date.Between(minBirthDate, maxBirthDate))
                .RuleFor(fa => fa.Height, f => f.Random.Number(180, 185))
                .RuleFor(fa => fa.Weight, f => f.Random.Number(80, 85));

            // Create an applicant object from the generated values.
            Applicant applicant = fakeApplicants;

            // Add the references to the applicant object.
            applicant.References = referencesList;

            // Add the applicant to the database.
            _dbContext.Applicants.Add(applicant);
        }

        // Save the changes to the database.
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Grants permissions for the read-only officer role.
    /// </summary>
    /// <param name="readOnlyOfficerRole">The read-only officer role.</param>
    private async Task GrantPermissionsForReadOnlyOfficer(ApplicationRole readOnlyOfficerRole)
    {
        // Define a list of permissions for the read-only officer role.
        var readOnlyOfficerPermissionsList = new List<ApplicationPermission>
        {
            new() { Name = "Applicants" },
            new() { Name = "Applicants.GetApplicant" },
            new() { Name = "Applicants.GetApplicants" },
            new() { Name = "Applicants.ExportAsPdf" },
            new() { Name = "Dashboard.GetHeadlinesData" }
        };

        // Get the permissions from the database that match the read-only officer's permission list.
        var readOnlyOfficerPermissions = (await _dbContext.ApplicationPermissions.ToListAsync())
            .Where(p => readOnlyOfficerPermissionsList.Any(rofp => rofp.Name == p.Name));

        // Add the matching permissions to the role claims for the read-only officer role.
        foreach (var permission in readOnlyOfficerPermissions)
            readOnlyOfficerRole.RoleClaims.Add(new ApplicationRoleClaim
            {
                ClaimType = "permissions",
                ClaimValue = permission.Name
            });
    }

    /// <summary>
    /// Grants permissions to the full-privileged officer role.
    /// </summary>
    /// <param name="fullPrivilegedOfficerRole">The full-privileged officer role.</param>
    private async Task GrantPermissionsForFullPrivilegedOfficerRole(ApplicationRole fullPrivilegedOfficerRole)
    {
        // Define a list of permissions for the full-privileged officer role.
        var fullPrivilegedOfficerPermissionsList = new List<ApplicationPermission>
        {
            new() { Name = "Applicants" },
            new() { Name = "Applicants.CreateApplicant" },
            new() { Name = "Applicants.DeleteApplicant" },
            new() { Name = "Applicants.GetApplicant" },
            new() { Name = "Applicants.GetApplicants" },
            new() { Name = "Applicants.UpdateApplicant" },
            new() { Name = "Applicants.ExportAsPdf" },
            new() { Name = "Dashboard.GetHeadlinesData" }
        };

        // Get the matching permissions from the database.
        var fullPrivilegedOfficerPermissions = (await _dbContext.ApplicationPermissions.ToListAsync())
            .Where(p => fullPrivilegedOfficerPermissionsList.Any(fpop => fpop.Name == p.Name));

        // Add the matching permissions to the role claims for the full-privileged officer role.
        foreach (var permission in fullPrivilegedOfficerPermissions)
            fullPrivilegedOfficerRole.RoleClaims.Add(new ApplicationRoleClaim
            {
                ClaimType = "permissions",
                ClaimValue = permission.Name
            });
    }

    /// <summary>
    /// Grants permissions to the host app admin role.
    /// </summary>
    /// <param name="hostAdminRole">The host app admin role.</param>
    private async Task GrantPermissionsForHostAppAdminRole(ApplicationRole hostAdminRole)
    {
        // Create a list of permissions to grant to the host admin role.
        var hostAdminPermissionsList = new List<ApplicationPermission>
                                       {
                                           new() { Name = "Tenants" },
                                           new() { Name = "Tenants.GetTenant" },
                                           new() { Name = "Tenants.GetTenants" },
                                           new() { Name = "Tenants.CreateTenant" },
                                           new() { Name = "Tenants.UpdateTenant" },
                                           new() { Name = "Tenants.DeleteTenant" }
                                       };

        // Retrieve the matching permissions from the database.
        var hostAdminPermissions = (await _dbContext.ApplicationPermissions.IgnoreQueryFilters().ToListAsync())
            .Where(p => hostAdminPermissionsList.Any(hap => hap.Name == p.Name));

        // Grant the permissions to the host admin role, if they don't already have them.
        foreach (var permission in hostAdminPermissions)
        {
            var isClaimExist = hostAdminRole.RoleClaims.Any(rc => rc.ClaimValue == permission.Name);

            // If the claim does not exist, add it to the host admin role.
            if (!isClaimExist)
                hostAdminRole.RoleClaims.Add(new ApplicationRoleClaim
                {
                    ClaimType = "permissions",
                    ClaimValue = permission.Name
                });
        }
    }

    /// <summary>
    /// Grants permissions to the admin role for single tenant mode.
    /// </summary>
    /// <param name="adminRole">The admin role to grant permissions to.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task GrantPermissionsForSingleTenantModeAdminRole(ApplicationRole adminRole)
    {
        // Retrieve permissions that are only visible to the tenants.
        var tenantPermissions = await _dbContext.ApplicationPermissions
                                                .IgnoreQueryFilters()
                                                .Where(p => p.TenantVisibility)
                                                .ToListAsync();

        // Iterate through each permission and add it to the admin role if it does not exist.
        foreach (var permission in tenantPermissions)
        {
            // Check if the permission already exists in the admin role.
            var isClaimExist = adminRole.RoleClaims.Any(rc => rc.ClaimValue == permission.Name);

            // If the permission does not exist, add it to the admin role.
            if (!isClaimExist)
                adminRole.RoleClaims.Add(new ApplicationRoleClaim { ClaimType = "permissions", ClaimValue = permission.Name });
        }
    }

    /// <summary>
    /// Disables password complexity requirements.
    /// </summary>
    private void DisablePasswordComplexity()
    {
        // Disable requiring a digit in the password.
        _identityOptions.Value.Password.RequireDigit = false;

        // Disable requiring a lowercase letter in the password.
        _identityOptions.Value.Password.RequireLowercase = false;

        // Disable requiring a non-alphanumeric character in the password.
        _identityOptions.Value.Password.RequireNonAlphanumeric = false;

        // Disable requiring an uppercase letter in the password.
        _identityOptions.Value.Password.RequireUppercase = false;
    }

    #endregion Private Methods
}