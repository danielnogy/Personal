using BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.CreateMaterialCategory;

namespace BinaryPlate.Application.Features.SSM.MaterialCategories.Commands.UpdateMaterialCategory;

public class UpdateMaterialCategoryCommandValidator : AbstractValidator<UpdateMaterialCategoryCommand>
{
    #region Public Constructors

    public UpdateMaterialCategoryCommandValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                                 .NotEmpty()
                                 .WithMessage("Denumirea categoriei este obligatorie");
    }

    #endregion Public Constructors
}