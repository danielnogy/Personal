using BinaryPlate.Domain.Entities.SSM;
using Mapster;

namespace BinaryPlate.Application.Features.SSM.QuestionCategories.Queries.GetQuestionCategories;

public class QuestionCategoryItem : AuditableDto
{
    #region Public Properties

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    #endregion Public Properties

    #region Public Methods

    public static QuestionCategoryItem MapFromEntity(QuestionCategory questionCategory)
    {
        return questionCategory.Adapt<QuestionCategoryItem>();
    }

    #endregion Public Methods
}