
### Developer Guide: Achieving Selective Data Access with Data-Centric Authorization

#### Overview:

This guide elaborates on the implementation of Data-Centric Authorization, focusing on achieving selective data access within a .NET application. Data-Centric Authorization emphasizes that authorization decisions are centered around the data itself, allowing for granular control over who can access specific data entities based on their roles and permissions. Additionally, this approach aligns with the principles of Role-Based Access Control (RBAC), where access permissions are determined by the roles assigned to users. Together, Data-Centric Authorization and RBAC provide a robust framework for managing access to sensitive data and ensuring compliance with security requirements.

#### Step 1: Define Roles:

Begin by defining roles within the ASP.NET Identity Core configuration to align with the desired access levels. These roles will denote different levels of access to CRUD operations on applicants. For instance, roles such as Individual, BusinessUnit, Division, and Organization can be defined.

```csharp
// Example roles definition:
public enum AccessRoles
{
    Individual,
    BusinessUnit,
    Division,
    Organization
}
```

#### Step 2: Assign Roles:

Assign appropriate roles to users based on their access levels. This assignment can occur during user registration or dynamically based on application logic. Utilize the `UserSeeder` class to create users with different access levels.

```csharp
public enum AccessRoles
{
    Individual,
    BusinessUnit,
    Division,
    Organization
}

public class UserSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserSeeder(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedUsers()
    {
        await SeedUser("user@example.com", AccessRoles.Individual);
        await SeedUser("businessunituser@example.com", AccessRoles.BusinessUnit);
        await SeedUser("divisionuser@example.com", AccessRoles.Division);
        await SeedUser("organizationuser@example.com", AccessRoles.Organization);
    }

    private async Task SeedUser(string email, AccessRoles accessRole)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser == null)
        {
            var newUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                // Other user properties...
            };

            await _userManager.CreateAsync(newUser, "P@ssw0rd");
            await _userManager.AddToRoleAsync(newUser, accessRole.ToString());
        }
    }
}
```

#### Step 3: Extend Applicant Object:

Extend the `Applicant` object by adding an `AccessLevelsList` property. This property should indicate the access levels permitted to interact with the applicant. Update the `Applicant` model accordingly.

```csharp
public class Applicant
{
    public int ApplicantId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    // Other applicant properties...

    /// <summary>
    /// Represents the access levels allowed to interact with this applicant.
    /// Access levels are mapped to specific roles and are used for Data-Centric Authorization.
    /// </summary>
    public List<int> AccessLevelsList { get; set; }

    public Applicant()
    {
        AccessLevelsList = new List<int>();
    }
}
```

#### Step 4: Implement CRUD Actions:

Implement CRUD actions within the `CRUDApplicantsController` to manage applicants. Ensure that data retrieval logic is based on the `AccessLevelsList` property, which matches the current logged-in user's roles. This ensures that users can only access data entities for which they have appropriate permissions.

```csharp
[BpAuthorize] // Requires authentication to access this API
[ApiController]
[Route("api/[controller]")]
public class CRUDApplicantsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CRUDApplicantsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    /// <summary>
    /// Retrieves an applicant by ID.
    /// </summary>
    [HttpPost("GetApplicant")]
    public async Task<IActionResult> GetApplicant(GetApplicantRequest request)
    {
        // Retrieve the current user
        var currentUser = await _userManager.GetUserAsync(User);
        // Get the access level based on the user's role
        var accessLevel = await GetAccessLevel(currentUser);

        // Retrieve the applicant by ID
        var applicant = await _context.Applicants.FindAsync(request.ApplicantId);

        // Check if the applicant exists
        if (applicant == null)
        {
            return NotFound();
        }

        // Check if the user has permission to access the applicant
        if (applicant.AccessLevelId != accessLevel.Id)
        {
            return Forbid();
        }

        return Ok(applicant);
    }

    /// <summary>
    /// Retrieves all applicants.
    /// </summary>
    [HttpPost("GetApplicants")]
    public async Task<IActionResult> GetApplicants()
    {
        // Retrieve the current user
        var currentUser = await _userManager.GetUserAsync(User);
        // Get the access level based on the user's role
        var accessLevel = await GetAccessLevel(currentUser);

        // Retrieve all applicants
        var applicants = await _context.Applicants
            .Where(a => a.AccessLevelId == accessLevel.Id)
            .ToListAsync();

        return Ok(applicants);
    }

    /// <summary>
    /// Creates a new applicant.
    /// </summary>
    [HttpPost("CreateApplicant")]
    public async Task<IActionResult> CreateApplicant(CreateApplicantRequest request)
    {
        // Retrieve the current user
        var currentUser = await _userManager.GetUserAsync(User);
        // Get the access level based on the user's role
        var accessLevel = await GetAccessLevel(currentUser);

        // Create the new applicant
        var applicant = new Applicant
        {
            Name = request.Name,
            Email = request.Email,
            // Set other applicant properties...
            AccessLevelId = accessLevel.Id
        };

        // Add the applicant to the database
        _context.Applicants.Add(applicant);
        await _context.SaveChangesAsync();

        return Ok(applicant);
    }

    /// <summary>
    /// Updates an existing applicant.
    /// </summary>
    [HttpPut("UpdateApplicant")]
    public async Task<IActionResult> UpdateApplicant(UpdateApplicantRequest request)
    {
        // Retrieve the current user
        var currentUser = await _userManager.GetUserAsync(User);
        // Get the access level based on the user's role
        var accessLevel = await GetAccessLevel(currentUser);

        // Retrieve the applicant by ID
        var applicant = await _context.Applicants.FindAsync(request.ApplicantId);

        // Check if the applicant exists
        if (applicant == null)
        {
            return NotFound();
        }

        // Check if the user has permission to update the applicant
        if (applicant.AccessLevelId != accessLevel.Id)
        {
            return Forbid();
        }

        // Update the applicant properties
        applicant.Name = request.Name;
        applicant.Email = request.Email;
        // Update other applicant properties...

        await _context.SaveChangesAsync();

        return Ok(applicant);
    }

    /// <summary>
    /// Deletes an existing applicant by ID.
    /// </summary>
    [HttpDelete("DeleteApplicantRequest")]
    public async Task<IActionResult> DeleteApplicant(DeleteApplicantRequest request)
    {
        // Retrieve the current user
        var currentUser = await _userManager.GetUserAsync(User);
        // Get the access level based on the user's role
        var accessLevel = await GetAccessLevel(currentUser);

        // Retrieve the applicant by ID
        var applicant = await _context.Applicants.FindAsync(request.ApplicantId);

        // Check if the applicant exists
        if (applicant == null)
        {
            return NotFound();
        }

        // Check if the user has permission to delete the applicant
        if (applicant.AccessLevelId != accessLevel.Id)
        {
            return Forbid();
        }

        // Remove the applicant from the database
        _context.Applicants.Remove(applicant);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes multiple applicants by their IDs.
    /// </summary>
    [HttpDelete("BulkDeleteApplicants")]
    public async Task<IActionResult> BulkDeleteApplicants(BulkDeleteApplicantsRequest request)
    {
        // Retrieve the current user
        var currentUser = await _userManager.GetUserAsync(User);
        // Get the access level based on the user's role
        var accessLevel = await GetAccessLevel(currentUser);

        // Retrieve the applicants to delete by their IDs
        var applicantsToDelete = await _context.Applicants
            .Where(a => request.ApplicantIds.Contains(a.ApplicantId))
            .ToListAsync();

        // Check if any applicants were found
        if (!applicantsToDelete.Any())
        {
            return NotFound();
        }

        // Check if the user has permission to delete each applicant
        foreach (var applicant in applicantsToDelete)
        {
            if (applicant.AccessLevelId != accessLevel.Id)
            {
                return Forbid();
            }
        }

        // Remove the applicants from the database
        _context.Applicants.RemoveRange(applicantsToDelete);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Retrieves the access level of the specified user.
    /// </summary>
    private async Task<AccessLevel> GetAccessLevel(ApplicationUser user)
    {
        // Retrieve the roles assigned to the user
        var roles = await _userManager.GetRolesAsync(user);
        // Find the highest access level among the user's roles
        var accessLevel = await _context.AccessLevels
            .Where(a => roles.Contains(a.Role))
            .OrderByDescending(a => a.Level)
            .FirstOrDefaultAsync();

        return accessLevel;
    }
}
```
In this implementation, you'll find comprehensive CRUD functionalities within the `CRUDApplicantsController`, ensuring efficient management of applicant data. Furthermore, the implementation incorporates helper methods to ascertain access levels, leveraging the user's roles for authorization checks. Additionally, both XML comments and inline comments are meticulously included to augment code clarity and serve as thorough documentation aids.

#### Conclusion:

By following this guide, you can implement Data-Centric Authorization to achieve selective data access within your .NET application. This approach ensures that authorization decisions are tailored to the characteristics and sensitivity of the data, providing granular control over who can access specific data entities based on their roles and permissions. Selective data access enhances security and ensures compliance with data privacy regulations.
