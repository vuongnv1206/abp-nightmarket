using FluentValidation;
using NightMarket.Admin.Catalogs.Manufacturers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Catalogs.Manufacturers.Validators
{
    public class CreateUpdateManufacturerDtoValidator : AbstractValidator<CreateUpdateManufacturerDto>
    {
        public CreateUpdateManufacturerDtoValidator()
        {
        }
    }
}
