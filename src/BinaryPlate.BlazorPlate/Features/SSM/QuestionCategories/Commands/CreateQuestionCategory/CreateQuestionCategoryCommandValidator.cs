namespace BinaryPlate.BlazorPlate.Features.SSM.QuestionCategories.Commands.CreateQuestionCategory;

public class CreateQuestionCategoryCommandValidator : AbstractValidator<CreateQuestionCategoryCommand>
{
    #region Public Constructors

    public CreateQuestionCategoryCommandValidator()
    {


        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Denumirea categoriei este obligatorie");

        //for relationship validator look into applicants validator
    }

    #endregion Public Constructors
}

