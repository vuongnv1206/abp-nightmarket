using FluentValidation;
using NightMarket.Admin.Products.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Products.Validators
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
