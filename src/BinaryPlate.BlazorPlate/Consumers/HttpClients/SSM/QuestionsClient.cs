using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionsAnswers;

namespace BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;

public class QuestionsClient(IHttpService httpService) : IQuestionsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetQuestionForEditResponse>> GetQuestion(GetQuestionForEditQuery request)
    {
        return await httpService.Post<GetQuestionForEditQuery, GetQuestionForEditResponse>("questions/GetQuestion", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<GetQuestionAnswersResponse>> GetQuestionAnswers(GetQuestionAnswersQuery getQuestionAnswersQuery)
    {
        return await httpService.Post<GetQuestionAnswersQuery, GetQuestionAnswersResponse>("questions/GetQuestionAnswers", getQuestionAnswersQuery);
    }


    public async Task<ApiResponseWrapper<GetQuestionsResponse>> GetQuestions(GetQuestionsQuery request)
    {
        return await httpService.Post<GetQuestionsQuery, GetQuestionsResponse>("questions/GetQuestions", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<CreateQuestionResponse>> CreateQuestion(CreateQuestionCommand request)
    {
        return await httpService.Post<CreateQuestionCommand, CreateQuestionResponse>("questions/CreateQuestion", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateQuestion(UpdateQuestionCommand request)
    {
        return await httpService.Put<UpdateQuestionCommand, string>("questions/UpdateQuestion", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> DeleteQuestion(int id)
    {
        return await httpService.Delete<string>($"questions/DeleteQuestion?id={id}", namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }


    #endregion Public Methods
}