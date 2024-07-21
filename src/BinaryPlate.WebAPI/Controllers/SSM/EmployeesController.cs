using BinaryPlate.Application.Features.SSM.Employees.Commands.CreateEmployee;
using BinaryPlate.Application.Features.SSM.Employees.Commands.DeleteEmployee;
using BinaryPlate.Application.Features.SSM.Employees.Commands.UpdateEmployee;
using BinaryPlate.Application.Features.SSM.Employees.Queries.GetEmployeeForEdit;
using BinaryPlate.Application.Features.SSM.Employees.Queries.GetEmployees;

namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize(TwoFactorAuthRequired = true)]
public class EmployeesController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetEmployeeForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetEmployee")]
    public async Task<IActionResult> GetEmployee(GetEmployeeForEditQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    

    [ProducesResponseType(typeof(ApiSuccessResponse<GetEmployeesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetEmployees")]
    public async Task<IActionResult> GetEmployees(GetEmployeesQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateEmployeeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateEmployee")]
    public async Task<IActionResult> CreateEmployee(CreateEmployeeCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateEmployee")]
    public async Task<IActionResult> UpdateEmployee(UpdateEmployeeCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteEmployee")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var response = await Sender.Send(new DeleteEmployeeCommand { Id = id });
        return TryGetResult(response);
    }

    //[ProducesResponseType(typeof(ApiSuccessResponse<ExportEmployeesResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("ExportAsPdf")]
    //public async Task<IActionResult> ExportAsPdf(ExportEmployeesQuery request)
    //{
    //    var response = await Sender.Send(request);
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}