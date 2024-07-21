
namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;

public class AnswerItemForEdit
{
    #region Public Properties

    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public DateTime? CreatedOn { get; set; }

    #endregion Public Properties
}