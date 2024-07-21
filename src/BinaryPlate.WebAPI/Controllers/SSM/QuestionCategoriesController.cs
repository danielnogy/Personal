using BinaryPlate.Application.Features.SSM.QuestionCategories.Commands.CreateQuestionCategory;
using BinaryPlate.Application.Features.SSM.QuestionCategories.Commands.DeleteQuestionCategory;
using BinaryPlate.Application.Features.SSM.QuestionCategories.Commands.UpdateQuestionCategory;
using BinaryPlate.Application.Features.SSM.QuestionCategories.Queries.GetQuestionCategoryForEdit;
using BinaryPlate.Application.Features.SSM.QuestionCategories.Queries.GetQuestionCategories;

namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize(TwoFactorAuthRequired = true)]
public class QuestionCategoriesController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetQuestionCategoryForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetQuestionCategory")]
    public async Task<IActionResult> GetQuestionCategory(GetQuestionCategoryForEditQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }
    [ProducesResponseType(typeof(ApiSuccessResponse<GetQuestionCategoriesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetQuestionCategories")]
    public async Task<IActionResult> GetQuestionCategories(GetQuestionCategoriesQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateQuestionCategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateQuestionCategory")]
    public async Task<IActionResult> CreateQuestionCategory(CreateQuestionCategoryCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateQuestionCategory")]
    public async Task<IActionResult> UpdateQuestionCategory(UpdateQuestionCategoryCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteQuestionCategory")]
    public async Task<IActionResult> DeleteQuestionCategory(int id)
    {
        var response = await Sender.Send(new DeleteQuestionCategoryCommand { Id = id });
        return TryGetResult(response);
    }

    //[ProducesResponseType(typeof(ApiSuccessResponse<ExportQuestionCategoriesResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("ExportAsPdf")]
    //public async Task<IActionResult> ExportAsPdf(ExportQuestionCategoriesQuery request)
    //{
    //    var response = await Sender.Send(request);
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}