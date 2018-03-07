// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Windows.Automation;
    using Xunit;

    [CaptureTestName]
    [Collection(nameof(SharedIntegrationHostFixture))]
    public abstract class AbstractIntegrationTest : IDisposable
    {
        public readonly VisualStudioInstance VisualStudio;

        private VisualStudioInstanceContext _visualStudioContext;

        protected AbstractIntegrationTest(VisualStudioInstanceFactory instanceFactory, Version version)
        {
            Automation.TransactionTimeout = 20000;
            _visualStudioContext = instanceFactory.GetNewOrUsedInstance(version, SharedIntegrationHostFixture.RequiredPackageIds);
            VisualStudio = _visualStudioContext.Instance;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _visualStudioContext.Dispose();
            }
        }
    }
}