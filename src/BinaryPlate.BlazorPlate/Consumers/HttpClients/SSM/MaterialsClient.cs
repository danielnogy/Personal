using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Commands.CreateMaterial;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Commands.UpdateMaterial;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterialForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterials;

namespace BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;

public class MaterialsClient(IHttpService httpService) : IMaterialsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetMaterialForEditResponse>> GetMaterial(GetMaterialForEditQuery request)
    {
        return await httpService.Post<GetMaterialForEditQuery, GetMaterialForEditResponse>("materials/GetMaterial", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }



    public async Task<ApiResponseWrapper<GetMaterialsResponse>> GetMaterials(GetMaterialsQuery request)
    {
        return await httpService.Post<GetMaterialsQuery, GetMaterialsResponse>("materials/GetMaterials", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<CreateMaterialResponse>> CreateMaterial(CreateMaterialCommand request)
    {
        return await httpService.Post<CreateMaterialCommand, CreateMaterialResponse>("materials/CreateMaterial", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateMaterial(UpdateMaterialCommand request)
    {
        return await httpService.Put<UpdateMaterialCommand, string>("materials/UpdateMaterial", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> DeleteMaterial(int id)
    {
        return await httpService.Delete<string>($"materials/DeleteMaterial?id={id}", namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }


    #endregion Public Methods
}