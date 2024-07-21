using BinaryPlate.BlazorPlate.Features.SSM.Materials.Commands.CreateMaterial;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Commands.UpdateMaterial;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterialForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Materials.Queries.GetMaterials;

namespace BinaryPlate.BlazorPlate.Contracts.Consumers.SSM
{
    public interface IMaterialsClient
    {
        Task<ApiResponseWrapper<CreateMaterialResponse>> CreateMaterial(CreateMaterialCommand request);
        Task<ApiResponseWrapper<string>> DeleteMaterial(int id);
        Task<ApiResponseWrapper<GetMaterialForEditResponse>> GetMaterial(GetMaterialForEditQuery request);
        Task<ApiResponseWrapper<GetMaterialsResponse>> GetMaterials(GetMaterialsQuery request);
        Task<ApiResponseWrapper<string>> UpdateMaterial(UpdateMaterialCommand request);
    }
}