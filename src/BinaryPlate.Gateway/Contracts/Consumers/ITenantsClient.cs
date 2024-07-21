namespace BinaryPlate.Gateway.Contracts.Consumers;

public interface ITenantsClient
{
    #region Public Methods

    Task<ApiResponseWrapper<CreateTenantResponse>> CreateTenant(CreateTenantCommand request);

    #endregion Public Methods
}