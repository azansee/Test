using System;
using JetBrains.Annotations;
using Volo.Abp.Collections;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.DependencyInjection
{
    public class OnServiceRegistredContext : IOnServiceRegistredContext
    {
        public virtual ITypeList<IAbpInterceptor> Interceptors { get; }

        public virtual Type ImplementationType { get; }

        public OnServiceRegistredContext([NotNull] Type implementationType)
        {
            ImplementationType = Check.NotNull(implementationType, nameof(implementationType));

            Interceptors = new TypeList<IAbpInterceptor>();
        }
    }
}