﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.Identity.EntityFrameworkCore
{
    [DependsOn(typeof(AbpIdentityDomainModule), typeof(AbpEntityFrameworkCoreModule))]
    public class AbpIdentityEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAbpDbContext<IdentityDbContext>(options =>
            {
                options.WithDefaultRepositories();
                options.WithCustomRepository<IdentityUser, EfCoreIdentityUserRepository>();
                options.WithCustomRepository<IdentityRole, EfCoreIdentityRoleRepository>();
            });

            services.AddAssemblyOf<AbpIdentityEntityFrameworkCoreModule>();
        }
    }
}
