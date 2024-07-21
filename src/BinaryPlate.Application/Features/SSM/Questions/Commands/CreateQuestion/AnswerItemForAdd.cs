using BinaryPlate.Domain.Entities.SSM;

namespace BinaryPlate.Application.Features.SSM.Questions.Commands.CreateQuestion;

public class AnswerItemForAdd
{
    #region Public Properties
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }

    #endregion Public Properties
}