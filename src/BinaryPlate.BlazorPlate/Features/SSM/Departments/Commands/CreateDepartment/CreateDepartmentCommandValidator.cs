namespace BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    #region Public Constructors

    public CreateDepartmentCommandValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Numele este obligatoriu");

        //for relationship validator look into applicants validator
    }

    #endregion Public Constructors
}

