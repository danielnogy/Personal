namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.CreateMaterialCategory;

public class CreateMaterialCategoryCommandValidator : AbstractValidator<CreateMaterialCategoryCommand>
{
    #region Public Constructors

    public CreateMaterialCategoryCommandValidator()
    {


        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Denumirea categoriei este obligatorie");
        
       //for relationship validator look into applicants validator
    }

    #endregion Public Constructors
}

