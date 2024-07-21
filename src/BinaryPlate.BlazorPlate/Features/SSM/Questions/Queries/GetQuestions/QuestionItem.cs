using BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionsAnswers;

namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;

public class QuestionItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Text { get; set; }
    public int CategoryId { get; set; }
    public List<QuestionAnswerItem> Answers { get; set; } = new();
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

}