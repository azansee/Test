﻿using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    //TODO: Renaming this project to Volo.Abp.MultiTenancy.Tests would be better!
    [DependsOn(typeof(AbpMultiTenancyAbstractionsModule))]
    public class MultiTenancyTestModule : AbpModule
    {
        
    }
}