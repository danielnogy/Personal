using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionForEdit;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;
using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionsAnswers;

namespace BinaryPlate.BlazorPlate.Contracts.Consumers.SSM
{
    public interface IQuestionsClient
    {
        Task<ApiResponseWrapper<CreateQuestionResponse>> CreateQuestion(CreateQuestionCommand request);
        Task<ApiResponseWrapper<string>> DeleteQuestion(int id);
        Task<ApiResponseWrapper<GetQuestionForEditResponse>> GetQuestion(GetQuestionForEditQuery request);
        Task<ApiResponseWrapper<GetQuestionAnswersResponse>> GetQuestionAnswers(GetQuestionAnswersQuery getQuestionAnswersQuery);
        Task<ApiResponseWrapper<GetQuestionsResponse>> GetQuestions(GetQuestionsQuery request);
        Task<ApiResponseWrapper<string>> UpdateQuestion(UpdateQuestionCommand request);
    }
}