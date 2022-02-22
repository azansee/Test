﻿using System;
using JetBrains.Annotations;

namespace Volo.Abp.Localization
{
    public class LocalizationResource
    {
        public Type ResourceType { get; }

        public string DefaultCultureName { get; set; }

        public ILocalizationDictionaryProvider DictionaryProvider
        {
            get => _dictionaryProvider;
            set
            {
                Check.NotNull(value, nameof(value));
                _dictionaryProvider = value;
            }
        }
        private ILocalizationDictionaryProvider _dictionaryProvider;

        public LocalizationResource([NotNull] Type resourceType, [NotNull] string defaultCultureName, [NotNull] ILocalizationDictionaryProvider dictionaryProvider)
        {
            Check.NotNull(resourceType, nameof(resourceType));
            Check.NotNull(defaultCultureName, nameof(defaultCultureName));
            Check.NotNull(dictionaryProvider, nameof(dictionaryProvider));

            ResourceType = resourceType;
            DefaultCultureName = defaultCultureName;
            DictionaryProvider = dictionaryProvider;
        }

        public virtual void Initialize(IServiceProvider serviceProvider) //TODO: Create a LocalizationResourceInitializationContext!
        {
            DictionaryProvider.Initialize(this);
        }
    }
}