﻿using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Volo.Abp.Permissions.EntityFrameworkCore
{
    public static class AbpPermissionsDbContextModelBuilderExtensions
    {
        public static void ConfigureAbpPermissions(
            [NotNull] this ModelBuilder builder,
            [CanBeNull] string tablePrefix = AbpPermissionConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = AbpPermissionConsts.DefaultDbSchema)
        {
            Check.NotNull(builder, nameof(builder));

            if (tablePrefix == null)
            {
                tablePrefix = "";
            }

            builder.Entity<PermissionGrant>(b =>
            {
                b.ToTable(tablePrefix + "PermissionGrants", schema);

                b.Property(x => x.Name).HasMaxLength(PermissionGrantConsts.MaxNameLength).IsRequired();
                b.Property(x => x.ProviderName).HasMaxLength(PermissionGrantConsts.MaxProviderNameLength).IsRequired();
                b.Property(x => x.ProviderKey).HasMaxLength(PermissionGrantConsts.MaxProviderKeyLength).IsRequired();

                b.HasIndex(x => new {x.Name, x.ProviderName, x.ProviderKey});
            });
        }
    }
}