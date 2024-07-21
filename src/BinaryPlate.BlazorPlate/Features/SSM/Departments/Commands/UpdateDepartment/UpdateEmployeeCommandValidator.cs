namespace BinaryPlate.BlazorPlate.Features.SSM.Departments.Commands.UpdateDepartment;

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
}