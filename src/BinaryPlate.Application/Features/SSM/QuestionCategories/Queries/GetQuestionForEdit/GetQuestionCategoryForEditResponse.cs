using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.QuestionCategories.Queries.GetQuestionCategoryForEdit;

public class GetQuestionCategoryForEditResponse : AuditableDto
{
    #region Public Properties
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetQuestionCategoryForEditResponse MapFromEntity(QuestionCategory questionCategory)
    {
        return questionCategory.Adapt<GetQuestionCategoryForEditResponse>();
    }

    #endregion Public Methods
}