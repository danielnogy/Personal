using BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest;
using BinaryPlate.Application.Features.SSM.Tests.Commands.DeleteTest;
using BinaryPlate.Application.Features.SSM.Tests.Commands.UpdateTest;
using BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestForEdit;
using BinaryPlate.Application.Features.SSM.Tests.Queries.GetTests;
using BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsMaterials;
using BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsQuestions;
using BinaryPlate.Application.Features.SSM.Tests.Queries.GetTestsResults;

namespace BinaryPlate.WebAPI.Controllers.SSM;

[Route("api/[controller]")]
[BpAuthorize(TwoFactorAuthRequired = true)]
public class TestsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetTestForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetTest")]
    public async Task<IActionResult> GetTest(GetTestForEditQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetTestMaterialsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetTestMaterials")]
    public async Task<IActionResult> GetTestReferences(GetTestMaterialsQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }
    [ProducesResponseType(typeof(ApiSuccessResponse<GetTestQuestionsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetTestQuestions")]
    public async Task<IActionResult> GetTestReferences(GetTestQuestionsQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }
    [ProducesResponseType(typeof(ApiSuccessResponse<GetTestResultsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetTestResults")]
    public async Task<IActionResult> GetTestReferences(GetTestResultsQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetTestsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetTests")]
    public async Task<IActionResult> GetTests(GetTestsQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<CreateTestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("CreateTest")]
    public async Task<IActionResult> CreateTest(CreateTestCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateTest")]
    public async Task<IActionResult> UpdateTest(UpdateTestCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpDelete("DeleteTest")]
    public async Task<IActionResult> DeleteTest(int id)
    {
        var response = await Sender.Send(new DeleteTestCommand { Id = id });
        return TryGetResult(response);
    }

    //[ProducesResponseType(typeof(ApiSuccessResponse<ExportTestsResponse>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    //[HttpPost("ExportAsPdf")]
    //public async Task<IActionResult> ExportAsPdf(ExportTestsQuery request)
    //{
    //    var response = await Sender.Send(request);
    //    return TryGetResult(response);
    //}

    #endregion Public Methods
}