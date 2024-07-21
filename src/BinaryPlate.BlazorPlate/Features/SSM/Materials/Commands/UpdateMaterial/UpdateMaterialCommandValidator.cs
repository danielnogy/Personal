namespace BinaryPlate.BlazorPlate.Features.SSM.Materials.Commands.UpdateMaterial;

public class UpdateMaterialCommandValidator : AbstractValidator<UpdateMaterialCommand>
{
    #region Public Constructors

    public UpdateMaterialCommandValidator()
    {

        RuleFor(v => v.Title).Cascade(CascadeMode.Stop)
                                .NotEmpty()
                                .WithMessage("Titlul este obligatoriu");
        RuleFor(v => v.Url).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Ruta este obligatorie");



    }

    #endregion Public Constructors

    #region Public Classes



    #endregion Public Classes
}