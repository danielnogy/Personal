using BinaryPlate.Application.Features.SSM.Departments.Commands.CreateDepartment;

namespace BinaryPlate.Application.Features.SSM.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    #region Public Constructors

    public UpdateDepartmentCommandValidator()
    {

        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Numele este obligatoriu");



    }

    #endregion Public Constructors

    #region Public Classes

    

    #endregion Public Classes
}