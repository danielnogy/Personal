namespace BinaryPlate.Infrastructure.Persistence;

public sealed class DbContextHelperService : IDbContextHelperService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUtcDateTimeProvider _utcDateTimeProvider;
    private readonly ITenantResolver _tenantResolver;

    public DbContextHelperService(IHttpContextAccessor httpContextAccessor,
                                  IUtcDateTimeProvider utcDateTimeProvider,
                                  ITenantResolver tenantResolver)
    {
        _httpContextAccessor = httpContextAccessor;
        _utcDateTimeProvider = utcDateTimeProvider;
        _tenantResolver = tenantResolver;
    }

    #region Public Methods

    public async Task PerformDataManagementActions(DbContext context)
    {
        // Encapsulate ChangeTracker entries for better readability.
        var allEntries = context.ChangeTracker.Entries();
        var auditableEntries = context.ChangeTracker.Entries<IAuditable>();
        var mustHaveTenantEntries = context.ChangeTracker.Entries<IMustHaveTenant>();
        var mayHaveTenantEntries = context.ChangeTracker.Entries<IMayHaveTenant>();
        var softDeletableEntries = context.ChangeTracker.Entries<ISoftDeletable>();
        var concurrencyStampEntries = context.ChangeTracker.Entries<IConcurrencyStamp>().Where(e => e.State == EntityState.Modified && e.Metadata.FindProperty("ConcurrencyStamp") != null);

        // Validate entries in the ChangeTracker.
        ValidateEntries(allEntries);

        // Set auditable properties for entities implementing IAuditable.
        SetAuditableProperties(auditableEntries, _httpContextAccessor.GetUserId(), _utcDateTimeProvider.GetUtcNow());

        // Set TenantId for entities implementing IMustHaveTenant and IMayHaveTenant.
        SetTenantIdForMultiTenant(mustHaveTenantEntries, mayHaveTenantEntries);

        // Set soft deletable state for entities implementing ISoftDeletable.
        SetSoftDeletableState(softDeletableEntries);

        // Update the concurrency stamp for entities implementing IConcurrencyStamp.
        await UpdateConcurrencyStamp(concurrencyStampEntries);
    }

    public void DeconfigureITenantEntities(ModelBuilder builder)
    {
        // Deconfigure entities implementing IMustHaveTenant interface.
        DeconfigureEntitiesByInterface<IMustHaveTenant>(builder);

        // Deconfigure entities implementing IMayHaveTenant interface.
        DeconfigureEntitiesByInterface<IMayHaveTenant>(builder);
    }

    public void ConfigureSettingsSchemaEntities(ModelBuilder builder)
    {
        // Maps the entity type to a table with a specific schema ("Settings") for all entities implementing ISettingsSchema.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(ISettingsSchema).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name, "Settings");
    }

    public void ConfigureMultiTenantsEntities(ModelBuilder builder)
    {
        // Set "TenantId" property as required for entities implementing "IMustHaveTenant" interface.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(IMustHaveTenant).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<Guid>("TenantId").IsRequired();

        // Set "TenantId" property as optional for entities implementing "IMayHaveTenant" interface.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(IMayHaveTenant).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<Guid?>("TenantId").IsRequired(false);
    }

    public void ConfigureSoftDeletableEntities(ModelBuilder builder)
    {
        // Create navigation or shadow properties for all entities implementing ISoftDeletable.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType)))
            builder.Entity(entityType.ClrType).Property<bool>("IsDeleted").IsRequired();

        // Filter out soft-deleted entities by default.
        builder.SetQueryFilter<ISoftDeletable>(p => EF.Property<bool>(p, "IsDeleted") == false);
    }

    public void ConfigureConcurrentEntities(ModelBuilder builder)
    {
        // Loop through all entity types, excluding certain types, and those without the "ConcurrencyStamp" property.
        foreach (var entityType in builder.Model.GetEntityTypes().Where(b => b.ClrType != typeof(Dictionary<string, object>) && b.GetProperties().All(p => p.Name != "ConcurrencyStamp")))
            // Add the "ConcurrencyStamp" property to the entity with the appropriate configuration.
            builder.Entity(entityType.ClrType)
                   .Property<string>("ConcurrencyStamp")
                   .IsConcurrencyToken();
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Validates entries in the ChangeTracker using data annotations.
    /// </summary>
    /// <param name="entries">The collection of EntityEntry instances to be validated.</param>
    private static void ValidateEntries(IEnumerable<EntityEntry> entries)
    {
        foreach (var entry in entries)
        {
            // Create a validation context for the current entry.
            var validationContext = new ValidationContext(entry);

            // Validate the object using data annotations.
            Validator.ValidateObject(entry, validationContext);
        }
    }

    /// <summary>
    /// Sets auditable properties (CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, DeletedOn, DeletedBy) based on the entity state.
    /// </summary>
    /// <param name="auditableEntries">The collection of EntityEntry instances implementing IAuditable.</param>
    /// <param name="userId">The user ID associated with the changes.</param>
    /// <param name="utcNow">The current UTC date and time.</param>
    private static void SetAuditableProperties(IEnumerable<EntityEntry<IAuditable>> auditableEntries, string userId, DateTime utcNow)
    {
        foreach (var entry in auditableEntries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // Set properties for added entities.
                    entry.Property("CreatedOn").CurrentValue = utcNow;
                    entry.Property("CreatedBy").CurrentValue = userId;
                    break;

                case EntityState.Modified:
                    // Set properties for modified entities.
                    entry.Property("ModifiedOn").CurrentValue = utcNow;
                    entry.Property("ModifiedBy").CurrentValue = userId;
                    break;

                case EntityState.Deleted:
                    // Set properties for deleted entities if they implement ISoftDeletable.
                    if (entry.Entity is ISoftDeletable deletableEntity)
                    {
                        entry.Property("DeletedOn").CurrentValue = utcNow;
                        entry.Property("DeletedBy").CurrentValue = userId;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Sets the state and properties for soft-deletable entities that are marked as deleted.
    /// </summary>
    /// <param name="softDeletableEntries">The collection of EntityEntry instances implementing ISoftDeletable.</param>
    private static void SetSoftDeletableState(IEnumerable<EntityEntry<ISoftDeletable>> softDeletableEntries)
    {
        foreach (var entry in softDeletableEntries.Where(x => x.State == EntityState.Deleted))
        {
            // Change the state to Unchanged to prevent actual deletion from the database.
            entry.State = EntityState.Unchanged;

            // Set the IsDeleted property to true.
            entry.Property("IsDeleted").CurrentValue = true;
        }
    }

    /// <summary>
    /// Updates the concurrency stamp for entities implementing IConcurrencyStamp.
    /// </summary>
    /// <param name="modifiedEntities">The collection of EntityEntry instances implementing IConcurrencyStamp.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private static Task UpdateConcurrencyStamp(IEnumerable<EntityEntry<IConcurrencyStamp>> modifiedEntities)
    {
        foreach (var entry in modifiedEntities)
        {
            // Get the original and current versions of the concurrency stamp.
            var originalVersion = entry.OriginalValues.GetValue<string>("ConcurrencyStamp");
            var currentVersion = entry.CurrentValues.GetValue<string>("ConcurrencyStamp");

            // Check for concurrency conflicts.
            if (originalVersion != currentVersion)
                throw new DbUpdateConcurrencyException(Resource.It_appears_someone_else_has_made_changes_or_deleted_the_data_you_re_updating);

            // Update the concurrency stamp with a new GUID value.
            entry.Property("ConcurrencyStamp").CurrentValue = Guid.NewGuid().ToString();
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Throws an exception if the provided tenant ID is null.
    /// </summary>
    /// <param name="tenantId">The tenant ID to be checked.</param>
    /// <returns>The non-null tenant ID.</returns>
    private static Guid? ThrowExceptionIfTenantIsNull(Guid? tenantId)
    {
        // Check if the tenant ID is null and throw an exception if true.
        if (!tenantId.HasValue)
            throw new InvalidOperationException(Resource.Tenant_is_not_found);

        // Return the non-null tenant ID.
        return tenantId.Value;
    }

    private void DeconfigureEntitiesByInterface<TInterface>(ModelBuilder builder) where TInterface : class
    {
        // Get the names of the properties in the specified interface.
        var interfaceProperties = typeof(TInterface).GetProperties().Select(p => p.Name).ToList();

        // Get the entity types that implement the specified interface.
        var entityTypes = builder.Model.GetEntityTypes().Where(t => typeof(TInterface).IsAssignableFrom(t.ClrType));

        foreach (var entity in entityTypes)
        {
            // Retrieve the builder for the current entity type.
            var entityTypeBuilder = builder.Entity(entity.ClrType);

            // Ignore the properties from the specified interface.
            foreach (var propertyName in interfaceProperties)
                entityTypeBuilder.Ignore(propertyName);
        }
    }

    /// <summary>
    /// Sets the TenantId property for multi-tenant entities based on their state.
    /// </summary>
    /// <param name="mustHaveTenantEntries">The collection of EntityEntry instances implementing IMustHaveTenant.</param>
    /// <param name="mayHaveTenantEntries">The collection of EntityEntry instances implementing IMayHaveTenant.</param>
    private void SetTenantIdForMultiTenant(IEnumerable<EntityEntry<IMustHaveTenant>> mustHaveTenantEntries,
                                           IEnumerable<EntityEntry<IMayHaveTenant>> mayHaveTenantEntries)
    {
        var currentTenantId = _tenantResolver.GetTenantId();

        if (_tenantResolver.TenantMode == TenantMode.MultiTenant)
        {
            foreach (var entry in mustHaveTenantEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // For added entities, set the tenant ID to the current tenant ID.
                        entry.Property("TenantId").CurrentValue = ThrowExceptionIfTenantIsNull(currentTenantId);
                        break;

                    case EntityState.Modified:
                        // For modified entities, if the tenant ID is null, set it to the current tenant ID.
                        entry.Property("TenantId").CurrentValue ??= ThrowExceptionIfTenantIsNull(currentTenantId);
                        break;
                }
            }

            foreach (var entry in mayHaveTenantEntries)
            {
                switch (entry.State)
                {
                    // For added entities, set the tenant ID to the current tenant ID.
                    case EntityState.Added:
                        entry.Property("TenantId").CurrentValue = currentTenantId;
                        break;

                    // For modified entities, set the tenant ID to the current tenant ID.
                    case EntityState.Modified:
                        entry.Property("TenantId").CurrentValue = currentTenantId;
                        break;
                }
            }
        }
    }

    #endregion Private Methods
}