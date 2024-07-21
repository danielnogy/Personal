using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestionsAnswers;

public class QuestionAnswerItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static QuestionAnswerItem MapFromEntity(Answer answer)
    {
        return answer.Adapt<QuestionAnswerItem>();
    }

    #endregion Public Methods
}