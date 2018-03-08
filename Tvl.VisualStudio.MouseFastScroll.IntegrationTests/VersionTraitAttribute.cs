// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using Xunit.Sdk;

    [TraitDiscoverer("Tvl.VisualStudio.MouseFastScroll.IntegrationTests.VersionTraitDiscoverer", "Tvl.VisualStudio.MouseFastScroll.IntegrationTests")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class VersionTraitAttribute : Attribute, ITraitAttribute
    {
        public VersionTraitAttribute(Type executionType)
        {
            ExecutionType = executionType;
        }

        public Type ExecutionType
        {
            get;
        }
    }
}
