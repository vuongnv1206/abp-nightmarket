using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NightMarket.Catalogs.Products;

namespace NightMarket.Configurations.Products
{
    public class ProductAttributeIntConfiguration : IEntityTypeConfiguration<ProductAttributeInt>
    {
        public void Configure(EntityTypeBuilder<ProductAttributeInt> builder)
        {
            builder.ToTable(NightMarketConsts.DbTablePrefix + "ProductAttributeInts");
            builder.HasKey(x => x.Id);
        }
    }
}
