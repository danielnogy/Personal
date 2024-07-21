namespace BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Commands.UpdateQuestionCategory;

public class UpdateQuestionCategoryCommand 
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties


}