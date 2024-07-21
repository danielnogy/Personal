namespace BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Queries.GetQuestionCategoryForEdit;

public class GetQuestionCategoryForEditResponse
{
    #region Public Properties
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}