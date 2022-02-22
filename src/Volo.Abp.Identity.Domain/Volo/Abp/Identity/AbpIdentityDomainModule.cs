﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpPermissionsDomainModule))]
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpIdentityDomainSharedModule))]
    public class AbpIdentityDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var identityBuilder = services.AddAbpIdentity();
            services.ExecutePreConfiguredActions(identityBuilder);

            services.AddAssemblyOf<AbpIdentityDomainModule>();
        }
    }
}