using System;
using System.Collections.Generic;

namespace Volo.Abp.Localization
{
    public abstract class LocalizationDictionaryProviderBase : ILocalizationDictionaryProvider
    {
        public IDictionary<string, ILocalizationDictionary> Dictionaries { get; }

        public event EventHandler Updated;

        protected LocalizationDictionaryProviderBase()
        {
            Dictionaries = new Dictionary<string, ILocalizationDictionary>();
        }

        public virtual void Initialize(LocalizationResourceInitializationContext context)
        {
        }

        public virtual void Extend(ILocalizationDictionaryProvider dictionaryProvider)
        {
            foreach (var dictionary in dictionaryProvider.Dictionaries.Values)
            {
                Extend(dictionary);
            }
        }

        protected virtual void Extend(ILocalizationDictionary dictionary)
        {
            var existingDictionary = Dictionaries.GetOrDefault(dictionary.CultureName);
            if (existingDictionary == null)
            {
                Dictionaries[dictionary.CultureName] = dictionary;
            }
            else
            {
                existingDictionary.Extend(dictionary);
            }
        }

        protected virtual void OnUpdated()
        {
            Updated.InvokeSafely(this);
        }
    }
}