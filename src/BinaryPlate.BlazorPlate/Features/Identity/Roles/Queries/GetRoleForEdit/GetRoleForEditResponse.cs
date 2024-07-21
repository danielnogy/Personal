namespace BinaryPlate.BlazorPlate.Features.Identity.Roles.Queries.GetRoleForEdit;

public class GetRoleForEditResponse
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}