using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Commands.CreateMaterialCategory;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Commands.UpdateMaterialCategory;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.MaterialCategories.Queries.GetMaterialCategoryForEdit;

namespace BinaryPlate.BlazorPlate.Contracts.Consumers.SSM
{
    public interface IMaterialCategoriesClient
    {
        Task<ApiResponseWrapper<CreateMaterialCategoryResponse>> CreateMaterialCategory(CreateMaterialCategoryCommand request);
        Task<ApiResponseWrapper<string>> DeleteMaterialCategory(int id);
        Task<ApiResponseWrapper<GetMaterialCategoriesResponse>> GetMaterialCategories(GetMaterialCategoriesQuery request);
        Task<ApiResponseWrapper<GetMaterialCategoryForEditResponse>> GetMaterialCategory(GetMaterialCategoryForEditQuery request);
        Task<ApiResponseWrapper<string>> UpdateMaterialCategory(UpdateMaterialCategoryCommand request);
    }
}