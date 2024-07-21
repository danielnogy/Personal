using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.CreateDepartment;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.UpdateDepartment;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartmentForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Departments.Queries.GetDepartments;

namespace BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;

public class DepartmentsClient(IHttpService httpService) :IDepartmentsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetDepartmentForEditResponse>> GetDepartment(GetDepartmentForEditQuery request)
    {
        return await httpService.Post<GetDepartmentForEditQuery, GetDepartmentForEditResponse>("departments/GetDepartment", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }



    public async Task<ApiResponseWrapper<GetDepartmentsResponse>> GetDepartments(GetDepartmentsQuery request)
    {
        return await httpService.Post<GetDepartmentsQuery, GetDepartmentsResponse>("departments/GetDepartments", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<CreateDepartmentResponse>> CreateDepartment(CreateDepartmentCommand request)
    {
        return await httpService.Post<CreateDepartmentCommand, CreateDepartmentResponse>("departments/CreateDepartment", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateDepartment(UpdateDepartmentCommand request)
    {
        return await httpService.Put<UpdateDepartmentCommand, string>("departments/UpdateDepartment", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> DeleteDepartment(int id)
    {
        return await httpService.Delete<string>($"departments/DeleteDepartment?id={id}", namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }


    #endregion Public Methods
}