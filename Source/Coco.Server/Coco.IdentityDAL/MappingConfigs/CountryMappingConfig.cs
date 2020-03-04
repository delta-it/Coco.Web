﻿using Coco.Entities.Constant;
using Coco.Entities.Domain.Dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coco.IdentityDAL.MappingConfigs
{
    public class CountryMappingConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable(nameof(Country), TableSchemaConst.DBO);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasMany(x => x.UserInfos)
                .WithOne(x => x.Country)
                .HasForeignKey(x => x.CountryId);
        }
    }
}