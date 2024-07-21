using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Commands.CreateEmployee;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Commands.UpdateEmployee;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Queries.GetEmployeeForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Employees.Queries.GetEmployees;

namespace BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;

public class EmployeesClient(IHttpService httpService) : IEmployeesClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetEmployeeForEditResponse>> GetEmployee(GetEmployeeForEditQuery request)
    {
        return await httpService.Post<GetEmployeeForEditQuery, GetEmployeeForEditResponse>("employees/GetEmployee", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }



    public async Task<ApiResponseWrapper<GetEmployeesResponse>> GetEmployees(GetEmployeesQuery request)
    {
        return await httpService.Post<GetEmployeesQuery, GetEmployeesResponse>("employees/GetEmployees", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<CreateEmployeeResponse>> CreateEmployee(CreateEmployeeCommand request)
    {
        return await httpService.Post<CreateEmployeeCommand, CreateEmployeeResponse>("employees/CreateEmployee", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateEmployee(UpdateEmployeeCommand request)
    {
        return await httpService.Put<UpdateEmployeeCommand, string>("employees/UpdateEmployee", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> DeleteEmployee(string id)
    {
        return await httpService.Delete<string>($"employees/DeleteEmployee?id={id}", namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }


    #endregion Public Methods
}