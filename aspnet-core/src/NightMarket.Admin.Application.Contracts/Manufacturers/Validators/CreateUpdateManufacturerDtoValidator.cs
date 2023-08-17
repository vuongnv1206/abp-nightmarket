using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Manufacturers.Validators
{
    public class CreateUpdateManufacturerDtoValidator : AbstractValidator<CreateUpdateManufacturerDto>
    {
        public CreateUpdateManufacturerDtoValidator()
        {
        }
    }
}
