using FluentValidation;
using NightMarket.Admin.Catalogs.Products.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Catalogs.Products.Validators
{
    public class AddUpdateProductAttributeDtoValidator : AbstractValidator<AddUpdateProductAttributeDto>
    {
        public AddUpdateProductAttributeDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.AttributeId).NotEmpty();

        }
    }
}
