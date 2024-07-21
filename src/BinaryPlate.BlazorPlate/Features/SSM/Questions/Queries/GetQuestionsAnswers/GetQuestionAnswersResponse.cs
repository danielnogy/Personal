namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestionsAnswers;

public class GetQuestionAnswersResponse
{
    #region Public Properties

    public PagedList<QuestionAnswerItem> Answers { get; set; }

    #endregion Public Properties
}