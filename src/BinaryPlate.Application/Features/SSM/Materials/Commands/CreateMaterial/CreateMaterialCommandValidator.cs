namespace BinaryPlate.Application.Features.SSM.Materials.Commands.CreateMaterial;

public class CreateMaterialCommandValidator : AbstractValidator<CreateMaterialCommand>
{
    #region Public Constructors

    public CreateMaterialCommandValidator()
    {
        RuleFor(v => v.Title).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Titlul este obligatoriu");
        RuleFor(v => v.Url).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Ruta este obligatorie");
        
       //for relationship validator look into applicants validator
    }

    #endregion Public Constructors
}

