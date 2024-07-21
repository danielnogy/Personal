namespace BinaryPlate.BlazorPlate.Features.Tenants.Commands.UpdateTenant;

public class UpdateTenantCommand
{
    #region Public Properties

    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}