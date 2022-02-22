﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AbpDesk.EntityFrameworkCore;
using AbpDesk.Web.Mvc.Navigation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.EmbeddedFiles;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.Autofac;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.Ui.Navigation;
using Volo.Abp.VirtualFileSystem;

namespace AbpDesk.Web.Mvc
{
    [DependsOn(
        typeof(AbpAspNetCoreEmbeddedFilesModule),
        typeof(AbpAspNetCoreMvcUiBootstrapModule),
        typeof(AbpDeskApplicationModule),
        typeof(AbpDeskEntityFrameworkCoreModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpAccountWebModule),
        typeof(AbpAutofacModule)
        )]
    public class AbpDeskWebMvcModule : AbpModule //TODO: Rename to AbpDeskWebModule, change default namespace to AbpDesk.Web
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            var hostingEnvironment = services.GetSingletonInstance<IHostingEnvironment>();
            var configuration = BuildConfiguration(hostingEnvironment);

            AbpDeskDbConfigurer.Configure(services, configuration);

            services.Configure<NavigationOptions>(options =>
            {
                options.MenuContributors.Add(new MainMenuContributor());
            });

            //services.Configure<RemoteServiceOptions>(configuration); //Needed when we use Volo.Abp.Identity.HttpApi.Client

            services.AddMvc() //TODO: Move to AbpAspNetCoreMvcModule!
                .AddViewLocalization()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/App");
                });

            services.AddAssemblyOf<AbpDeskWebMvcModule>();

            services.Configure<BundlingOptions>(options =>
            {
                options.ScriptBundles.Add("GlobalScripts", new[]
                {
                    "/Abp/ApplicationConfigurationScript?_v=" + DateTime.Now.Ticks,
                    "/Abp/ServiceProxyScript?_v=" + DateTime.Now.Ticks
                });
            });

            services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(AbpDeskApplicationModule).Assembly);
            });

            var env = services.GetSingletonInstance<IHostingEnvironment>(); //TODO: Find a better way!


            if (env.IsDevelopment())
            {
                services.Configure<VirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiModule>(Path.Combine(env.ContentRootPath, "..\\..\\Volo.Abp.AspNetCore.Mvc.UI"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiBootstrapModule>(Path.Combine(env.ContentRootPath, "..\\..\\Volo.Abp.AspNetCore.Mvc.UI.Bootstrap"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAccountWebModule>(Path.Combine(env.ContentRootPath, "..\\..\\Volo.Abp.Account.Web"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpIdentityWebModule>(Path.Combine(env.ContentRootPath, "..\\..\\Volo.Abp.Identity.Web"));
                });
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseVirtualFiles();

            app.UseAuthentication();

            var cultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("tr")
            };

            //TODO: Should we add this to the framework, or left it to the application?
            //TODO: Should we add this as the first middleware (to support localization in all middlewares too)?
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en"),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static IConfigurationRoot BuildConfiguration(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
