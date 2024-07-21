namespace BinaryPlate.BlazorPlate.Features.SSM.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    #region Public Constructors

    public CreateEmployeeCommandValidator()
    {
        RuleFor(v => v.Email).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .EmailAddress()
                                 .WithMessage("Email invalid");
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Numele este obligatoriu");

        //for relationship validator look into applicants validator
    }

    #endregion Public Constructors
}

