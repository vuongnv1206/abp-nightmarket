using FluentValidation;
using NightMarket.Admin.Catalogs.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Catalogs.Products.Validators
{
    public class CreateUpdateProductDtoValidator : AbstractValidator<CreateUpdateProductDto>
    {
        public CreateUpdateProductDtoValidator()
        {
        }
    }
}
