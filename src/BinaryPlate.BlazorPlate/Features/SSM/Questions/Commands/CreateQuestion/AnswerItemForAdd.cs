namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;

public class AnswerItemForAdd
{
    #region Public Properties
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }

    #endregion Public Properties


}