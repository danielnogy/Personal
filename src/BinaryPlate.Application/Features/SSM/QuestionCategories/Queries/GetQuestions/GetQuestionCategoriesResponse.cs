namespace BinaryPlate.Application.Features.SSM.QuestionCategories.Queries.GetQuestionCategories;

public class GetQuestionCategoriesResponse
{
    #region Public Properties

    public PagedList<QuestionCategoryItem> QuestionCategories { get; set; }

    #endregion Public Properties
}