using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightMarket.Catalogs.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightMarket.Configurations.Inventories
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> b)
        {
            b.ToTable(NightMarketConsts.DbTablePrefix + "Inventories");
            b.HasKey(x => x.Id);
            b.Property(x => x.SKU)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            b.Property(x => x.StockQuantity)
                .IsRequired();
        }
    }
}
