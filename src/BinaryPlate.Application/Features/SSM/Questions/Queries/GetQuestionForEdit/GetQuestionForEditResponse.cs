using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.Questions.Queries.GetQuestionForEdit;

public class GetQuestionForEditResponse : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public string Text { get; set; }
    public int CategoryId { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetQuestionForEditResponse MapFromEntity(Question question)
    {
        return question.Adapt<GetQuestionForEditResponse>();
    }

    #endregion Public Methods
}