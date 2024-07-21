using BinaryPlate.Domain.Entities.SSM;

namespace BinaryPlate.Application.Common.Contracts.Infrastructure.Persistence;

/// <inheritdoc cref="DbContext" />
public interface IApplicationDbContext : IDisposable
{
    #region Public Properties

    // User-related entities.
    DbSet<ApplicationUser> Users { get; set; }

    DbSet<ApplicationRole> Roles { get; set; }
    DbSet<ApplicationUserRole> UserRoles { get; set; }
    DbSet<ApplicationUserClaim> UserClaims { get; set; }
    DbSet<ApplicationUserLogin> UserLogins { get; set; }
    DbSet<ApplicationRoleClaim> RoleClaims { get; set; }
    DbSet<ApplicationUserToken> UserTokens { get; set; }
    DbSet<ApplicationUserAttachment> ApplicationUserAttachments { get; set; }
    DbSet<ApplicationPermission> ApplicationPermissions { get; set; }

    // Application configuration entities.
    DbSet<UserSettings> UserSettings { get; set; }

    DbSet<PasswordSettings> PasswordSettings { get; set; }
    DbSet<LockoutSettings> LockoutSettings { get; set; }
    DbSet<SignInSettings> SignInSettings { get; set; }
    DbSet<TokenSettings> TokenSettings { get; set; }
    DbSet<FileStorageSettings> FileStorageSettings { get; set; }

    // Application-specific entities.
    DbSet<Applicant> Applicants { get; set; }

    DbSet<Reference> References { get; set; }

    // Application-generic entities.
    DbSet<Report> Reports { get; set; }

    DbSet<Tenant> Tenants { get; set; }

    // DbContext-related properties.
    DbContext Current { get; }

    DatabaseFacade Database { get; }
    DbSet<Test> Tests { get; set; }
    DbSet<TestResult> TestResults { get; set; }
    DbSet<Material> Materials { get; set; }
    DbSet<TestMaterial> TestMaterials { get; set; }
    DbSet<Question> Questions { get; set; }
    DbSet<TestQuestion> TestQuestions { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<Answer> Answers { get; set; }
    DbSet<QuestionCategory> QuestionCategories { get; set; }
    DbSet<Department> Departments { get; set; }
    DbSet<MaterialCategory> MaterialCategories { get; set; }

    #endregion Public Properties

    #region Public Methods

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Ensures the creation of the tenant database.
    /// </summary>
    /// <returns>A boolean value indicating whether the tenant database was created successfully.</returns>
    Task<bool> EnsureTenantDatabaseCreated();

    /// <summary>
    /// Attempts to switch the current database context to the host database.
    /// </summary>
    /// <returns>True if the switch is successful, otherwise false.</returns>
    bool TrySwitchToHostDatabase();

    /// <summary>
    /// Attempts to rename the tenant database from the current name to a new name.
    /// </summary>
    /// <param name="currentDbName">The current name of the tenant database.</param>
    /// <param name="newDbName">The new name for the tenant database.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>True if the rename is successful, otherwise false.</returns>
    Task<bool> TryRenameTenantDatabase(string currentDbName, string newDbName, CancellationToken cancellationToken);

    /// <summary>
    /// Attempts to switch the current database context to the specified tenant database.
    /// </summary>
    /// <param name="tenantName">The name of the target tenant for which to switch the database context.</param>
    /// <returns>True if the switch is successful, otherwise false.</returns>
    bool TrySwitchToTenantDatabase(string tenantName);


    #endregion Public Methods
}