using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Products.Validators
{
    public class CreateUpdateProductDtoValidator : AbstractValidator<CreateUpdateProductDto>
    {
        public CreateUpdateProductDtoValidator()
        {
        }
    }
}
