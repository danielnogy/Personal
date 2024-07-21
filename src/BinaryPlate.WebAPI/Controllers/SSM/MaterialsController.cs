using BinaryPlate.Application.Features.SSM.Materials.Commands.CreateMaterial;
using BinaryPlate.Application.Features.SSM.Materials.Commands.DeleteMaterial;
using BinaryPlate.Application.Features.SSM.Materials.Commands.UpdateMaterial;
using BinaryPlate.Application.Features.SSM.Materials.Queries.GetMaterialForEdit;
using BinaryPlate.Application.Features.SSM.Materials.Queries.GetMaterials;

namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize(TwoFactorAuthRequired = true)]
public class MaterialsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetMaterialForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetMaterial")]
    public async Task<IActionResult> GetMaterial(GetMaterialForEditQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }
    [ProducesResponseType(typeof(ApiSuccessResponse<GetMaterialsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetMaterials")]
    public async Task<IActionResult> GetMaterials(GetMaterialsQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateMaterialResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateMaterial")]
    public async Task<IActionResult> CreateMaterial(CreateMaterialCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateMaterial")]
    public async Task<IActionResult> UpdateMaterial(UpdateMaterialCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteMaterial")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var response = await Sender.Send(new DeleteMaterialCommand { Id = id });
        return TryGetResult(response);
    }

    //[ProducesResponseType(typeof(ApiSuccessResponse<ExportMaterialsResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("ExportAsPdf")]
    //public async Task<IActionResult> ExportAsPdf(ExportMaterialsQuery request)
    //{
    //    var response = await Sender.Send(request);
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}