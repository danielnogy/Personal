using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetQuestionForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTests;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsMaterials;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsQuestions;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsResults;

namespace BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;

public class TestsClient(IHttpService httpService) : ITestsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetTestForEditResponse>> GetTest(GetTestForEditQuery request)
    {
        return await httpService.Post<GetTestForEditQuery, GetTestForEditResponse>("tests/GetTest", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<GetTestMaterialsResponse>> GetTestMaterials(GetTestMaterialsQuery getTestMaterialsQuery)
    {
        return await httpService.Post<GetTestMaterialsQuery, GetTestMaterialsResponse>("tests/GetTestMaterials", getTestMaterialsQuery);
    }
    public async Task<ApiResponseWrapper<GetTestQuestionsResponse>> GetTestQuestions(GetTestQuestionsQuery getTestQuestionsQuery)
    {
        return await httpService.Post<GetTestQuestionsQuery, GetTestQuestionsResponse>("tests/GetTestQuestions", getTestQuestionsQuery);
    }
    public async Task<ApiResponseWrapper<GetTestResultsResponse>> GetTestResults(GetTestResultsQuery getTestResultsQuery)
    {
        return await httpService.Post<GetTestResultsQuery, GetTestResultsResponse>("tests/GetTestResults", getTestResultsQuery);
    }

    public async Task<ApiResponseWrapper<GetTestsResponse>> GetTests(GetTestsQuery request)
    {
        return await httpService.Post<GetTestsQuery, GetTestsResponse>("tests/GetTests", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<CreateTestResponse>> CreateTest(CreateTestCommand request)
    {
        return await httpService.Post<CreateTestCommand, CreateTestResponse>("tests/CreateTest", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateTest(UpdateTestCommand request)
    {
        return await httpService.Put<UpdateTestCommand, string>("tests/UpdateTest", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> DeleteTest(int id)
    {
        return await httpService.Delete<string>($"tests/DeleteTest?id={id}", namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }


    #endregion Public Methods
}