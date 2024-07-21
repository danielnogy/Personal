using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Commands.CreateQuestionCategory;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Commands.UpdateQuestionCategory;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoryForEdit;

namespace BinaryPlate.BlazorPlate.Contracts.Consumers.SSM
{
    public interface IQuestionCategoriesClient
    {
        Task<ApiResponseWrapper<CreateQuestionCategoryResponse>> CreateQuestionCategory(CreateQuestionCategoryCommand request);
        Task<ApiResponseWrapper<string>> DeleteQuestionCategory(int id);
        Task<ApiResponseWrapper<GetQuestionCategoriesResponse>> GetQuestionCategories(GetQuestionCategoriesQuery request);
        Task<ApiResponseWrapper<GetQuestionCategoryForEditResponse>> GetQuestionCategory(GetQuestionCategoryForEditQuery request);
        Task<ApiResponseWrapper<string>> UpdateQuestionCategory(UpdateQuestionCategoryCommand request);
    }
}