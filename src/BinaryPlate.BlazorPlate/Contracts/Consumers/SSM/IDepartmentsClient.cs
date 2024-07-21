using BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.CreateDepartment;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.UpdateDepartment;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartmentForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartments;

namespace BinaryPlate.BlazorPlate.Contracts.Consumers.SSM
{
    public interface IDepartmentsClient
    {
        Task<ApiResponseWrapper<CreateDepartmentResponse>> CreateDepartment(CreateDepartmentCommand request);
        Task<ApiResponseWrapper<string>> DeleteDepartment(int id);
        Task<ApiResponseWrapper<GetDepartmentForEditResponse>> GetDepartment(GetDepartmentForEditQuery request);
        Task<ApiResponseWrapper<GetDepartmentsResponse>> GetDepartments(GetDepartmentsQuery request);
        Task<ApiResponseWrapper<string>> UpdateDepartment(UpdateDepartmentCommand request);
    }
}