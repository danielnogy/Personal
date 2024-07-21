namespace BinaryPlate.Application.Features.SSM.Tests.Commands.CreateTest;

public class CreateTestCommandValidator : AbstractValidator<CreateTestCommand>
{
    #region Public Constructors

    public CreateTestCommandValidator()
    {


        RuleFor(v => v.Title).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Titlul este obligatoriu");

        
       //for relationship validator look into applicants validator
    }

    #endregion Public Constructors
}

