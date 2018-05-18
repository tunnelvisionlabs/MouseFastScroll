// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using System;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading.WpfFactDiscoverer", "Tvl.VisualStudio.MouseFastScroll.IntegrationTests")]
    public class WpfFactAttribute : FactAttribute
    {
    }
}
