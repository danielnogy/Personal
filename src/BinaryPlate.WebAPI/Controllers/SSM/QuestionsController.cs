using BinaryPlate.Application.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.Application.Features.SSM.Questions.Commands.DeleteQuestion;
using BinaryPlate.Application.Features.SSM.Questions.Commands.UpdateQuestion;
using BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestionForEdit;
using BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestions;
using BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestionsAnswers;

namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize(TwoFactorAuthRequired = true)]
public class QuestionsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetQuestionForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetQuestion")]
    public async Task<IActionResult> GetQuestion(GetQuestionForEditQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetAnswersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetQuestionAnswers")]
    public async Task<IActionResult> GetQuestionReferences(Application.Features.SSM.Questions.Queries.GetQuestionsAnswers.GetQuestionAnswersQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetQuestionsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetQuestions")]
    public async Task<IActionResult> GetQuestions(GetQuestionsQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateQuestionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateQuestion")]
    public async Task<IActionResult> CreateQuestion(CreateQuestionCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateQuestion")]
    public async Task<IActionResult> UpdateQuestion(UpdateQuestionCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteQuestion")]
    public async Task<IActionResult> DeleteQuestion(int id)
    {
        var response = await Sender.Send(new DeleteQuestionCommand { Id = id });
        return TryGetResult(response);
    }

    //[ProducesResponseType(typeof(ApiSuccessResponse<ExportQuestionsResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("ExportAsPdf")]
    //public async Task<IActionResult> ExportAsPdf(ExportQuestionsQuery request)
    //{
    //    var response = await Sender.Send(request);
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}