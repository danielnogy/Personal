using BinaryPlate.BlazorPlate.Features.SSM.Employees.Commands.CreateEmployee;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Commands.UpdateEmployee;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Queries.GetEmployeeForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Queries.GetEmployees;

namespace BinaryPlate.BlazorPlate.Contracts.Consumers.SSM
{
    public interface IEmployeesClient
    {
        Task<ApiResponseWrapper<CreateEmployeeResponse>> CreateEmployee(CreateEmployeeCommand request);
        Task<ApiResponseWrapper<string>> DeleteEmployee(string id);
        Task<ApiResponseWrapper<GetEmployeeForEditResponse>> GetEmployee(GetEmployeeForEditQuery request);
        Task<ApiResponseWrapper<GetEmployeesResponse>> GetEmployees(GetEmployeesQuery request);
        Task<ApiResponseWrapper<string>> UpdateEmployee(UpdateEmployeeCommand request);
    }
}