using BinaryPlate.Application.Features.SSM.Departments.Commands.CreateDepartment;
using BinaryPlate.Application.Features.SSM.Departments.Commands.DeleteDepartment;
using BinaryPlate.Application.Features.SSM.Departments.Commands.UpdateDepartment;
using BinaryPlate.Application.Features.SSM.Departments.Queries.GetDepartmentForEdit;
using BinaryPlate.Application.Features.SSM.Departments.Queries.GetDepartments;

namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize(TwoFactorAuthRequired = true)]
public class DepartmentsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetDepartmentForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetDepartment")]
    public async Task<IActionResult> GetDepartment(GetDepartmentForEditQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }
    [ProducesResponseType(typeof(ApiSuccessResponse<GetDepartmentsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetDepartments")]
    public async Task<IActionResult> GetDepartments(GetDepartmentsQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateDepartmentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateDepartment")]
    public async Task<IActionResult> CreateDepartment(CreateDepartmentCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateDepartment")]
    public async Task<IActionResult> UpdateDepartment(UpdateDepartmentCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteDepartment")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var response = await Sender.Send(new DeleteDepartmentCommand { Id = id });
        return TryGetResult(response);
    }

    //[ProducesResponseType(typeof(ApiSuccessResponse<ExportDepartmentsResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("ExportAsPdf")]
    //public async Task<IActionResult> ExportAsPdf(ExportDepartmentsQuery request)
    //{
    //    var response = await Sender.Send(request);
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}