namespace BinaryPlate.Infrastructure.Services.Identity;

public class PermissionScanner(ApplicationPartManager partManager, IApplicationDbContext dbContext) : IPermissionScanner
{
    // Initialize the PermissionScanner with required dependencies.

    #region Public Methods

    public async Task InitializeDefaultPermissions()
    {
        // Delete existing application permissions.
        await DeleteApplicationPermissions();

        // Get filtered controllers for each authorization type.
        var authorizeControllers = GetControllersByAttributeType("BpAuthorize");
        var tenantAuthorizeControllers = GetControllersByAttributeType("BpTenantAuthorize");
        var hostAuthorizeControllers = GetControllersByAttributeType("BpHostAuthorize");

        // Add the root node to the database.
        var rootNode = await AddRootNode("Actions");

        // Add root permissions for each authorization type.
        var rootPermissions = await AddRootPermissions(authorizeControllers, rootNode, true, true);
        var rootTenantPermissions = await AddRootPermissions(tenantAuthorizeControllers, rootNode, true, false);
        var rootHostPermissions = await AddRootPermissions(hostAuthorizeControllers, rootNode, false, true);

        // Add child permissions for each root permission.
        await AddChildPermissions(rootNode, rootPermissions, authorizeControllers);
        await AddChildPermissions(rootNode, rootTenantPermissions, tenantAuthorizeControllers);
        await AddChildPermissions(rootNode, rootHostPermissions, hostAuthorizeControllers);

        try
        {
            // Save changes to the database.
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Handle database update exception with a specific error message.
            throw new DbUpdateException("Error occurred while updating the database. Please check the data and try again.", ex);
        }
    }

    #endregion Public Methods

    #region Private Methods

    private async Task DeleteApplicationPermissions()
    {
        // Retrieve and delete existing application permissions.
        // Filter permissions based on the condition that they are not custom.
        // Ignore query filters to ensure soft-deleted permissions are considered.
        var permissionsToBeDeleted = await dbContext.ApplicationPermissions
                                                    .Where(p => !p.IsCustomPermission)
                                                    .IgnoreQueryFilters()
                                                    .ToListAsync();

        // Remove the retrieved permissions from the database.
        dbContext.ApplicationPermissions.RemoveRange(permissionsToBeDeleted);
    }

    private List<dynamic> GetControllersByAttributeType(string authorizeAttribute)
    {
        // Retrieve and filter controllers based on the authorization attribute.
        var feature = new ControllerFeature();

        // Populate the feature with controllers from the provided part manager.
        partManager.PopulateFeature(feature);

        return feature.Controllers
                      .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                      .Where(m => !m.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
                      .Select(info => new
                      {
                          // Extract controller name, action name, and attribute information.
                          Controller = info.DeclaringType?.Name.Replace("Controller", string.Empty),
                          Action = info.Name,
                          Attributes = string.Join(",", info.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))),
                          TypeAttributes = string.Join(",", info.DeclaringType?.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")) ?? Array.Empty<string>())
                      })
                      // Filter controllers based on attribute conditions.
                      .Where(c => !c.TypeAttributes.Contains("AllowAnonymous") && c.TypeAttributes.Contains(authorizeAttribute))
                      .Where(c => !c.Attributes.Contains("AllowAnonymous"))
                      // Order the result by controller and action names.
                      .OrderBy(c => c.Controller).ThenBy(c => c.Action).ToList<dynamic>();
    }

    private async Task<ApplicationPermission> AddRootNode(string nodeName)
    {
        // Create a new root node with the specified name and visibility properties.
        var rootNode = new ApplicationPermission { Name = nodeName, TenantVisibility = true, HostVisibility = true };

        // Asynchronously add the root node to the ApplicationPermissions DbSet in the dbContext.
        await dbContext.ApplicationPermissions.AddAsync(rootNode);

        // Return the created root node.
        return rootNode;
    }

    private async Task<List<ApplicationPermission>> AddRootPermissions(List<dynamic> controllers, ApplicationPermission rootNode, bool tenantVisibility, bool hostVisibility)
    {
        // Initialize a list to store the added root permissions.
        var rootPermissions = new List<ApplicationPermission>();

        // Iterate through each controller in the filtered controllers.
        foreach (var item in controllers)
        {
            // Generate the name for the root permission based on the controller.
            var rootPermissionName = $"{item.Controller}";

            // Check if a root permission with the same name does not exist in the rootPermissions list.
            if (rootPermissions.All(p => p.Name != rootPermissionName))
            {
                // Create a new root permission with the generated name, associated parent ID, and visibility properties.
                var rootPermission = new ApplicationPermission
                {
                    Name = rootPermissionName,
                    ParentId = rootNode.Id == Guid.Empty ? (await dbContext.ApplicationPermissions.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Name == rootNode.Name))?.Id : rootNode.Id,
                    TenantVisibility = tenantVisibility,
                    HostVisibility = hostVisibility
                };

                // Add the new root permission to the rootPermissions list.
                rootPermissions.Add(rootPermission);

                // Asynchronously add the new root permission to the ApplicationPermissions DbSet in the dbContext.
                await dbContext.ApplicationPermissions.AddAsync(rootPermission);
            }
        }

        // Return the list of added root permissions.
        return rootPermissions;
    }

    private async Task AddChildPermissions(ApplicationPermission rootNode, List<ApplicationPermission> rootPermissions, List<dynamic> controllers)
    {
        // Iterate through each root permission in the provided rootPermissions list.
        foreach (var rootPermission in rootPermissions)
        {
            // Set the ParentId of the root permission based on the root node's Id or the specified root node's Id.
            rootPermission.ParentId = rootNode.Id == Guid.Empty ? dbContext.ApplicationPermissions.FirstOrDefault(p => p.Name == rootNode.Name)?.Id : rootNode.Id;

            // Iterate through each controller in the specified controllers list where the controller matches the root permission's name.
            foreach (var type in controllers.Where(ct => ct.Controller == rootPermission.Name))
            {
                // Generate the name for the child permission in the format "Controller.Action".
                var childPermissionName = $"{type.Controller}.{type.Action}";

                // Extract the controller name from the generated child permission name.
                var controllerName = childPermissionName.Split(".")[0];

                // Asynchronously add a new child permission to the ApplicationPermissions DbSet in the dbContext.
                await dbContext.ApplicationPermissions.AddAsync(new ApplicationPermission
                {
                    Name = childPermissionName,
                    // Set the ParentId of the child permission based on the root permission's Id or the specified root permission's Id.
                    ParentId = rootPermission.Id == Guid.Empty ? (await dbContext.ApplicationPermissions.FirstOrDefaultAsync(p => p.Name == controllerName))?.Id :
                        rootPermission.Id,
                    // Set the visibility properties of the child permission based on the root permission.
                    TenantVisibility = rootPermission.TenantVisibility,
                    HostVisibility = rootPermission.HostVisibility
                });
            }
        }
    }

    #endregion Private Methods
}