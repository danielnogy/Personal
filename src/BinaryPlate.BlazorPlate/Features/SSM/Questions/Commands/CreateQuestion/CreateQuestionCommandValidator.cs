namespace BinaryPlate.BlazorPlate.Features.SSM.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    #region Public Constructors

    public CreateQuestionCommandValidator()
    {


        RuleFor(v => v.Text).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Textul intrebarii este obligatoriu");

        //for relationship validator look into applicants validator
    }

    #endregion Public Constructors
}

