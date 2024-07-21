

using BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;
using Mapster;

namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionsAnswers;

public class QuestionAnswerItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }

    #endregion Public Properties
    public AnswerItemForAdd MapToAdd(QuestionAnswerItem questionAnswerItem)
    {
        return questionAnswerItem.Adapt<AnswerItemForAdd>();
    }

}