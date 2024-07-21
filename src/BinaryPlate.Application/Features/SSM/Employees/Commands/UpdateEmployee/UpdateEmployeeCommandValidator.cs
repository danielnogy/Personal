using BinaryPlate.Application.Features.SSM.Employees.Commands.CreateEmployee;

namespace BinaryPlate.Application.Features.SSM.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    #region Public Constructors

    public UpdateEmployeeCommandValidator()
    {

        RuleFor(v => v.Email).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .EmailAddress()
                                 .WithMessage("Email invalid");
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Numele este obligatoriu");



    }

    #endregion Public Constructors

    #region Public Classes

    

    #endregion Public Classes
}