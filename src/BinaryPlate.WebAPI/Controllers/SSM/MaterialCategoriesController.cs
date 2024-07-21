using BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.CreateMaterialCategory;
using BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.DeleteMaterialCategory;
using BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.UpdateMaterialCategory;
using BinaryPlate.Application.Features.SSM.MaterialCategories.Queries.GetMaterialCategoryForEdit;
using BinaryPlate.Application.Features.SSM.MaterialCategories.Queries.GetMaterialCategories;

namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize(TwoFactorAuthRequired = true)]
public class MaterialCategoriesController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetMaterialCategoryForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetMaterialCategory")]
    public async Task<IActionResult> GetMaterialCategory(GetMaterialCategoryForEditQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }
    [ProducesResponseType(typeof(ApiSuccessResponse<GetMaterialCategoriesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetMaterialCategories")]
    public async Task<IActionResult> GetMaterialCategories(GetMaterialCategoriesQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateMaterialCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateMaterialCategory")]
    public async Task<IActionResult> CreateMaterialCategory(CreateMaterialCategoryCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateMaterialCategory")]
    public async Task<IActionResult> UpdateMaterialCategory(UpdateMaterialCategoryCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteMaterialCategory")]
    public async Task<IActionResult> DeleteMaterialCategory(int id)
    {
        var response = await Sender.Send(new DeleteMaterialCategoryCommand { Id = id });
        return TryGetResult(response);
    }

    //[ProducesResponseType(typeof(ApiSuccessResponse<ExportMaterialCategoriesResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("ExportAsPdf")]
    //public async Task<IActionResult> ExportAsPdf(ExportMaterialCategoriesQuery request)
    //{
    //    var response = await Sender.Send(request);
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}