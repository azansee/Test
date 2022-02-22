﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Volo.Abp.MultiTenancy.ConfigurationStore;
using Xunit;

namespace Volo.Abp.Data.MultiTenancy
{
    public class MultiTenantConnectionStringResolver_Tests : MultiTenancyTestBase
    {
        private readonly IMultiTenancyManager _multiTenancyManager;
        private readonly IConnectionStringResolver _connectionResolver;

        public MultiTenantConnectionStringResolver_Tests()
        {
            _multiTenancyManager = ServiceProvider.GetRequiredService<IMultiTenancyManager>();

            _connectionResolver = ServiceProvider.GetRequiredService<IConnectionStringResolver>();
            _connectionResolver.ShouldBeOfType<MultiTenantConnectionStringResolver>();
        }

        protected override void BeforeAddApplication(IServiceCollection services)
        {
            services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = "default-value";
                options.ConnectionStrings["db1"] = "db1-default-value";
            });

            services.Configure<ConfigurationTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantInformation(Guid.NewGuid(), "tenant1")
                    {
                        ConnectionStrings =
                        {
                            { ConnectionStrings.DefaultConnectionStringName, "tenant1-default-value" },
                            { "db1", "tenant1-db1-value" }
                        }
                    },
                    new TenantInformation(Guid.NewGuid(), "tenant2")
                };
            });
        }

        [Fact]
        public void All_Tests()
        {
            //No tenant in current context
            _connectionResolver.Resolve().ShouldBe("default-value");
            _connectionResolver.Resolve("db1").ShouldBe("db1-default-value");

            //Overrided connection strings for tenant1
            using (_multiTenancyManager.ChangeTenant("tenant1"))
            {
                _connectionResolver.Resolve().ShouldBe("tenant1-default-value");
                _connectionResolver.Resolve("db1").ShouldBe("tenant1-db1-value");
            }

            //No tenant in current context
            _connectionResolver.Resolve().ShouldBe("default-value");
            _connectionResolver.Resolve("db1").ShouldBe("db1-default-value");

            //Undefined connection strings for tenant2
            using (_multiTenancyManager.ChangeTenant("tenant2"))
            {
                _connectionResolver.Resolve().ShouldBe("default-value");
                _connectionResolver.Resolve("db1").ShouldBe("db1-default-value");
            }
        }
    }
}
