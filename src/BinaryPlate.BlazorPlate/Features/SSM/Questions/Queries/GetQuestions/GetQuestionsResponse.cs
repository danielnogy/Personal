namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Queries.GetQuestions;

public class GetQuestionsResponse
{
    #region Public Properties

    public PagedList<QuestionItem> Questions { get; set; }

    #endregion Public Properties
}