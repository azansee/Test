﻿using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Modularity
{
    public abstract class AbpModule : 
        IAbpModule,
        IOnPreApplicationInitialization,
        IOnApplicationInitialization,
        IOnPostApplicationInitialization,
        IOnApplicationShutdown, 
        IPreConfigureServices, 
        IPostConfigureServices
    {
        protected internal bool SkipAutoServiceRegistration { get; protected set; }

        protected internal ServiceConfigurationContext ServiceConfigurationContext
        {
            get
            {
                if (_serviceConfigurationContext == null)
                {
                    throw new AbpException($"{nameof(ServiceConfigurationContext)} is only available in the {nameof(PreConfigureServices)}, {nameof(PreConfigureServices)} and {nameof(PreConfigureServices)} methods.");
                }

                return _serviceConfigurationContext;
            }
            internal set => _serviceConfigurationContext = value;
        }

        private ServiceConfigurationContext _serviceConfigurationContext;

        public virtual void PreConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public virtual void ConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public virtual void PostConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public virtual void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            
        }

        public virtual void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            
        }

        public virtual void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {

        }

        public virtual void OnApplicationShutdown(ApplicationShutdownContext context)
        {

        }

        public static bool IsAbpModule(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IAbpModule).GetTypeInfo().IsAssignableFrom(type);
        }

        internal static void CheckAbpModuleType(Type moduleType)
        {
            if (!IsAbpModule(moduleType))
            {
                throw new ArgumentException("Given type is not an ABP module: " + moduleType.AssemblyQualifiedName);
            }
        }

        protected void Configure<TOptions>(Action<TOptions> configureOptions) 
            where TOptions : class
        {
            ServiceConfigurationContext.Services.Configure(configureOptions);
        }

        protected void PreConfigure<TOptions>(Action<TOptions> configureOptions)
            where TOptions : class
        {
            ServiceConfigurationContext.Services.PreConfigure(configureOptions);
        }
    }
}