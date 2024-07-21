using BinaryPlate.Application.Features.SSM.QuestionCategories.Commands.CreateQuestionCategory;

namespace BinaryPlate.Application.Features.SSM.QuestionCategories.Commands.UpdateQuestionCategory;

public class UpdateQuestionCategoryCommandValidator : AbstractValidator<UpdateQuestionCategoryCommand>
{
    #region Public Constructors

    public UpdateQuestionCategoryCommandValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Denumirea categoriei este obligatorie");
    }

    #endregion Public Constructors
}