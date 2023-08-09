using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightMarket.ProductAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightMarket.Configurations.ProductAttributes
{
    public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductAttribute> b)
        {
            b.ToTable(NightMarketConsts.DbTablePrefix + "Attributes");
            b.HasKey(x => x.Id);
            b.Property(x => x.Code)
                .HasMaxLength(AttributeConst.MaxCodeLength)
                .IsUnicode(false)
                .IsRequired();

            b.Property(x => x.Label)
                .HasMaxLength(AttributeConst.MaxLabelLength)
                .IsRequired();
        }
    }
}
