namespace BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestionsAnswers;

public class GetAnswersResponse
{
    #region Public Properties

    public PagedList<QuestionAnswerItem> Answers { get; set; }

    #endregion Public Properties
}