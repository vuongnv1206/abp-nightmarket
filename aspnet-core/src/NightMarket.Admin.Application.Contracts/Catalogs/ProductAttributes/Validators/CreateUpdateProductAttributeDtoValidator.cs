using FluentValidation;
using NightMarket.Admin.Catalogs.ProductAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Catalogs.ProductAttributes.Validators
{
    public class CreateUpdateProductAttributeDtoValidator : AbstractValidator<CreateUpdateProductAttributeDto>
    {
        public CreateUpdateProductAttributeDtoValidator()
        {
            RuleFor(x => x.Label).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.DataType).NotNull();
            //RuleFor(x => x.SeoMetaDescription).NotEmpty().MaximumLength(250);
        }
    }
}
