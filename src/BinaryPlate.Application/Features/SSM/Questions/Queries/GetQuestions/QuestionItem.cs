using BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestionsAnswers;
using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestions;

public class QuestionItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Text { get; set; }
    public int CategoryId { get; set; }
    public List<QuestionAnswerItem> Answers { get; set; } = new();

    #endregion Public Properties

    #region Public Methods

    public static QuestionItem MapFromEntity(Question question, bool withAnswers = false)
    {
        var questionItem = question.Adapt<QuestionItem>();
        if (withAnswers)
            return questionItem;

        questionItem.Answers = new();
        return questionItem;
    }

    #endregion Public Methods
}