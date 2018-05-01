// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Automation;
    using Xunit;

    [CaptureTestName]
    [Collection(nameof(SharedIntegrationHostFixture))]
    public abstract class AbstractIntegrationTest : IAsyncLifetime, IDisposable
    {
        private readonly VisualStudioInstanceFactory _instanceFactory;
        private readonly Version _version;
        private VisualStudioInstanceContext _visualStudioContext;

        protected AbstractIntegrationTest(VisualStudioInstanceFactory instanceFactory, Version version)
        {
            Assert.Equal(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());
            _instanceFactory = instanceFactory;
            _version = version;
            Automation.TransactionTimeout = 20000;
        }

        public VisualStudioInstance VisualStudio => _visualStudioContext?.Instance;

        public virtual async Task InitializeAsync()
        {
            _visualStudioContext = await _instanceFactory.GetNewOrUsedInstanceAsync(_version, SharedIntegrationHostFixture.RequiredPackageIds).ConfigureAwait(false);
        }

        public Task DisposeAsync()
        {
            return Task.FromResult<object>(null);
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
