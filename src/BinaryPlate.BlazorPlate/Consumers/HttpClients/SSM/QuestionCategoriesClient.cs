using BinaryPlate.BlazorPlate.Contracts.Consumers.SSM;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Commands.CreateQuestionCategory;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Commands.UpdateQuestionCategory;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoriess;
using BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoryForEdit;

namespace BinaryPlate.BlazorPlate.Consumers.HttpClients.SSM;

public class QuestionCategoriesClient(IHttpService httpService) : IQuestionCategoriesClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetQuestionCategoryForEditResponse>> GetQuestionCategory(GetQuestionCategoryForEditQuery request)
    {
        return await httpService.Post<GetQuestionCategoryForEditQuery, GetQuestionCategoryForEditResponse>("questionCategories/GetQuestionCategory", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }



    public async Task<ApiResponseWrapper<GetQuestionCategoriesResponse>> GetQuestionCategories(GetQuestionCategoriesQuery request)
    {
        return await httpService.Post<GetQuestionCategoriesQuery, GetQuestionCategoriesResponse>("questionCategories/GetQuestionCategories", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<CreateQuestionCategoryResponse>> CreateQuestionCategory(CreateQuestionCategoryCommand request)
    {
        return await httpService.Post<CreateQuestionCategoryCommand, CreateQuestionCategoryResponse>("questionCategories/CreateQuestionCategory", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> UpdateQuestionCategory(UpdateQuestionCategoryCommand request)
    {
        return await httpService.Put<UpdateQuestionCategoryCommand, string>("questionCategories/UpdateQuestionCategory", request, namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }

    public async Task<ApiResponseWrapper<string>> DeleteQuestionCategory(int id)
    {
        return await httpService.Delete<string>($"questionCategories/DeleteQuestionCategory?id={id}", namedHttpClient: NamedHttpClient.TwoFactorAuthClient);
    }


    #endregion Public Methods
}