using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Commands.CreateMaterialCategory;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Commands.UpdateMaterialCategory;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoryForEdit;

namespace BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;

public class MaterialCategoriesClient(IHttpService httpService) : IMaterialCategoriesClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetMaterialCategoryForEditResponse>> GetMaterialCategory(GetMaterialCategoryForEditQuery request)
    {
        return await httpService.Post<GetMaterialCategoryForEditQuery, GetMaterialCategoryForEditResponse>("materialCategories/GetMaterialCategory", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }



    public async Task<ApiResponseWrapper<GetMaterialCategoriesResponse>> GetMaterialCategories(GetMaterialCategoriesQuery request)
    {
        return await httpService.Post<GetMaterialCategoriesQuery, GetMaterialCategoriesResponse>("materialCategories/GetMaterialCategories", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<CreateMaterialCategoryResponse>> CreateMaterialCategory(CreateMaterialCategoryCommand request)
    {
        return await httpService.Post<CreateMaterialCategoryCommand, CreateMaterialCategoryResponse>("materialCategories/CreateMaterialCategory", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateMaterialCategory(UpdateMaterialCategoryCommand request)
    {
        return await httpService.Put<UpdateMaterialCategoryCommand, string>("materialCategories/UpdateMaterialCategory", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> DeleteMaterialCategory(int id)
    {
        return await httpService.Delete<string>($"materialCategories/DeleteMaterialCategory?id={id}", namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }


    #endregion Public Methods
}