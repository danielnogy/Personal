
namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionForEdit;

public class GetQuestionForEditResponse 
{
    #region Public Properties
    public int Id { get; set; }
    public string Text { get; set; }
    public int CategoryId { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties


}