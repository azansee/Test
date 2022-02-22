﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.SettingManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(AbpSettingManagementTestBaseModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule)
        )]
    public class AbpSettingManagementEntityFrameworkCoreTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var sqliteConnection = CreateDatabaseAndGetConnection();

            context.Services.Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(abpDbContextConfigurationContext =>
                {
                    abpDbContextConfigurationContext.DbContextOptions.UseSqlite(sqliteConnection);
                });
            });

            context.Services.AddAssemblyOf<AbpSettingManagementEntityFrameworkCoreTestModule>();
        }

        private static SqliteConnection CreateDatabaseAndGetConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            new AbpSettingManagementDbContext(
                new DbContextOptionsBuilder<AbpSettingManagementDbContext>().UseSqlite(connection).Options
            ).GetService<IRelationalDatabaseCreator>().CreateTables();

            return connection;
        }
    }
}