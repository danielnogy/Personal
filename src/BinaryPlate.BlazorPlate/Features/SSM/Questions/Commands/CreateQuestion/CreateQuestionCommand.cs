
namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;

public class CreateQuestionCommand 
{
    #region Public Properties

    public string Text { get; set; }
    public int CategoryId { get; set; }

    public IReadOnlyList<AnswerItemForAdd> AnswerItems { get; set; } = new List<AnswerItemForAdd>();

    #endregion Public Properties

    
}