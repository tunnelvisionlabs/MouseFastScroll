// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using System;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [XunitTestCaseDiscoverer("Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading.IdeFactDiscoverer", "Tvl.VisualStudio.MouseFastScroll.IntegrationTests")]
    public class IdeFactAttribute : FactAttribute
    {
        public IdeFactAttribute()
        {
            MinVersion = VisualStudioVersion.VS2012;
            MaxVersion = VisualStudioVersion.VS2017;
        }

        public VisualStudioVersion MinVersion
        {
            get;
            set;
        }

        public VisualStudioVersion MaxVersion
        {
            get;
            set;
        }
    }
}
