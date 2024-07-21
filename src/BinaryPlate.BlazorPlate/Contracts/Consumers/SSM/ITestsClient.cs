using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.CreateTest;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Commands.UpdateTest;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetQuestionForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTests;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsMaterials;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsQuestions;
using BinaryPlate.BlazorPlate.Features.SSM.Tests.Queries.GetTestsResults;

namespace BinaryPlate.BlazorPlate.Contracts.Consumers.SSM
{
    public interface ITestsClient
    {
        Task<ApiResponseWrapper<CreateTestResponse>> CreateTest(CreateTestCommand request);
        Task<ApiResponseWrapper<string>> DeleteTest(int id);
        Task<ApiResponseWrapper<GetTestForEditResponse>> GetTest(GetTestForEditQuery request);
        Task<ApiResponseWrapper<GetTestMaterialsResponse>> GetTestMaterials(GetTestMaterialsQuery getTestMaterialsQuery);
        Task<ApiResponseWrapper<GetTestQuestionsResponse>> GetTestQuestions(GetTestQuestionsQuery getTestQuestionsQuery);
        Task<ApiResponseWrapper<GetTestResultsResponse>> GetTestResults(GetTestResultsQuery getTestResultsQuery);
        Task<ApiResponseWrapper<GetTestsResponse>> GetTests(GetTestsQuery request);
        Task<ApiResponseWrapper<string>> UpdateTest(UpdateTestCommand request);
    }
}