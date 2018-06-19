// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class VersionTraitDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            var executionType = (Type)traitAttribute.GetConstructorArguments().Single();
            if (executionType is null)
            {
                yield return new KeyValuePair<string, string>("Category", "VSUnknown");
            }
            else
            {
                yield return new KeyValuePair<string, string>("Category", executionType.Name);
            }
        }
    }
}
