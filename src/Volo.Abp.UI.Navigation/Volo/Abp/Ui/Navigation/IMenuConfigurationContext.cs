using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Ui.Navigation
{
    public interface IMenuConfigurationContext : IServiceProviderAccessor
    {
        ApplicationMenu Menu { get; set; }

        //TODO: Add Localization, Authorization components since they are most used components on menu creation!
    }
}