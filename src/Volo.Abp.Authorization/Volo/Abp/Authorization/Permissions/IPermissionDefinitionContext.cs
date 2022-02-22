﻿using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions
{
    public interface IPermissionDefinitionContext
    {
        //TODO: Add Get methods to find and modify a permission or group.

        PermissionGroupDefinition AddGroup([NotNull] string name);
    }
}