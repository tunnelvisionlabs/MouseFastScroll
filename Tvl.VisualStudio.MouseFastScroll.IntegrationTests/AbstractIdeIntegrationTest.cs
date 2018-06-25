// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Xunit;

    [CaptureTestName]
    [Collection(nameof(SharedIntegrationHostFixture))]
    public abstract class AbstractIdeIntegrationTest : IAsyncLifetime, IDisposable
    {
        protected AbstractIdeIntegrationTest()
        {
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
