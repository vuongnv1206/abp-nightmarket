using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightMarket.IdentitySettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightMarket.Configurations.IdentitySettings
{
	public class IdentitySettingConfiguration : IEntityTypeConfiguration<IdentitySetting>
	{
		public void Configure(EntityTypeBuilder<IdentitySetting> b)
		{
			b.ToTable(NightMarketConsts.DbTablePrefix + "IdentitySettings");
			b.HasKey(x => x.Id);
			b.Property(x => x.Name)
				.HasMaxLength(200)
				.IsUnicode(false)
				.IsRequired();
		}
	}
}
