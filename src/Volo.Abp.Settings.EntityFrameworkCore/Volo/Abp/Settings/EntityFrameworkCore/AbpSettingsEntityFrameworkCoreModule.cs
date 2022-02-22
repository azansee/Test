﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Settings.EntityFrameworkCore
{
    [DependsOn(typeof(AbpSettingsDomainModule))]
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class AbpSettingsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<AbpSettingsDbContext>(options =>
            {
                options.AddDefaultRepositories<IAbpSettingsDbContext>();

                options.AddRepository<Setting, EfCoreSettingRepository>();
            });

            services.AddAssemblyOf<AbpSettingsEntityFrameworkCoreModule>();
        }
    }
}
