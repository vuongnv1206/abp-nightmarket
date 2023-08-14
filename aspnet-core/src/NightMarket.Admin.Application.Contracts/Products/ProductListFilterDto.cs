using NightMarket.Admin.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace NightMarket.Admin.Products
{
    public class ProductListFilterDto : BaseListFilterDto
    {
        public Guid? CategoryId { get; set; }
    }
}
