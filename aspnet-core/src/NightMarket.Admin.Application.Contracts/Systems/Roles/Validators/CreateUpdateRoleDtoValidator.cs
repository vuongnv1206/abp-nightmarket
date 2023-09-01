using FluentValidation;
using NightMarket.Admin.Systems.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Systems.Roles.Validators
{
    public class CreateUpdateRoleDtoValidator : AbstractValidator<CreateUpdateRoleDto>
    {
        public CreateUpdateRoleDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
