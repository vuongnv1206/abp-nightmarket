﻿using NightMarket.Admin.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Products.Attributes
{
	public class ProductAttributeListFilterDto : BaseListFilterDto
	{
        public Guid ProductId { get; set; }
    }
}
