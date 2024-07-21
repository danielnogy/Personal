﻿namespace BinaryPlate.BlazorPlate.Features.Identity.Roles.Commands.UpdateRole;

public class RoleForEditValidator : AbstractValidator<GetRoleForEditResponse>
{
    #region Public Constructors

    public RoleForEditValidator()
    {
        RuleFor(v => v.Name).Cascade(CascadeMode.Stop)
                            .NotEmpty()
                            .WithMessage(Resource.Role_name_is_required);
    }

    #endregion Public Constructors
}