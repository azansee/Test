﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc.Authorization;
using Volo.Abp.AspNetCore.TestBase;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.MemoryDb;
using Volo.Abp.Modularity;
using Volo.Abp.TestApp;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(
        typeof(AbpAspNetCoreTestBaseModule),
        typeof(AbpMemoryDbTestModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpAspNetCoreMvcTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddLocalization(); //TODO: Move to the framework..?

            context.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MyClaimTestPolicy", policy =>
                {
                    policy.RequireClaim("MyCustomClaimType", "42");
                });
            });

            context.Services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(TestAppModule).Assembly, opts =>
                {
                    opts.UrlActionNameNormalizer = urlActionNameNormalizerContext =>
                        string.Equals(urlActionNameNormalizerContext.ActionNameInUrl, "phone", StringComparison.OrdinalIgnoreCase)
                            ? "phones"
                            : urlActionNameNormalizerContext.ActionNameInUrl;
                });
            });

            context.Services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<TestPermissionDefinitionProvider>();
            });

            context.Services.AddAssemblyOf<AbpAspNetCoreMvcTestModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseMiddleware<FakeAuthenticationMiddleware>();
            app.UseAuditing();
            app.UseUnitOfWork();
            app.UseMvcWithDefaultRoute();
        }
    }
}