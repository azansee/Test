﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Settings.EntityFrameworkCore
{
    public class EfCoreSettingRepository : EfCoreRepository<IAbpSettingsDbContext, Setting, Guid>, ISettingRepository
    {
        public EfCoreSettingRepository(IDbContextProvider<IAbpSettingsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Setting> FindAsync(string name, string providerName, string providerKey)
        {
            return await DbSet.FirstOrDefaultAsync(s => s.Name == name && s.ProviderName == providerName && s.ProviderKey == providerKey);
        }

        public async Task<List<Setting>> GetListAsync(string providerName, string providerKey)
        {
            return await DbSet.Where(s => s.ProviderName == providerName && s.ProviderKey == providerKey).ToListAsync();
        }
    }
}
