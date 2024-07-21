using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;


namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommand 
{
    #region Public Properties

    public int Id { get; set; }
    public string Text { get; set; }
    public int CategoryId { get; set; }
    public string ConcurrencyStamp { get; set; }

    public List<AnswerItemForAdd> NewAnswers { get; set; } = new();
    public List<AnswerItemForEdit> ModifiedAnswers { get; set; } = new();
    public List<int> RemovedAnswers { get; set; } = new();

    #endregion Public Properties




}