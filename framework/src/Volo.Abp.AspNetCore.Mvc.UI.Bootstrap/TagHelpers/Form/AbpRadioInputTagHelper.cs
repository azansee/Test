﻿using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form
{
    public class AbpRadioInputTagHelper : AbpTagHelper<AbpRadioInputTagHelper, AbpRadioInputTagHelperService>
    {
        public ModelExpression AspFor { get; set; }

        public string Label { get; set; }

        public bool? Inline { get; set; }

        public bool? Disabled { get; set; }

        public IEnumerable<SelectListItem> AspItems { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public AbpRadioInputTagHelper(AbpRadioInputTagHelperService tagHelperService)
            : base(tagHelperService)
        {

        }
    }
}
