using FluentValidation;
using NightMarket.Admin.ProductCategories;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.ProductCategories.Validators
{
    public class CreateUpdateProductCategoryDtoValidator : AbstractValidator<CreateUpdateProductCategoryDto>
    {
        public CreateUpdateProductCategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Slug).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(50);
            RuleFor(x => x.CoverPicture).MaximumLength(250);
            RuleFor(x => x.SeoMetaDescription).NotEmpty().MaximumLength(250);

        }
    }
}
