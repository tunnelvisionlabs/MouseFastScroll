// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Microsoft.VisualStudio.ComponentModelHost;
    using Microsoft.VisualStudio.Threading;
    using Xunit;
    using ServiceProvider = Microsoft.VisualStudio.Shell.ServiceProvider;

    [VsTestSettings(UIThread = true)]
    public abstract class AbstractIdeIntegrationTest : IAsyncLifetime, IDisposable
    {
        private JoinableTaskContext _joinableTaskContext;
        private JoinableTaskCollection _joinableTaskCollection;
        private JoinableTaskFactory _joinableTaskFactory;

        protected AbstractIdeIntegrationTest()
        {
            Assert.True(Application.Current.Dispatcher.CheckAccess());

            var componentModel = (IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel));
            JoinableTaskContext = componentModel.GetExtensions<JoinableTaskContext>().SingleOrDefault() ?? new JoinableTaskContext();
        }

        protected JoinableTaskContext JoinableTaskContext
        {
            get
            {
                return _joinableTaskContext ?? throw new InvalidOperationException();
            }

            private set
            {
                if (value == _joinableTaskContext)
                {
                    return;
                }

                if (value is null)
                {
                    _joinableTaskContext = null;
                    _joinableTaskCollection = null;
                    _joinableTaskFactory = null;
                }
                else
                {
                    _joinableTaskContext = value;
                    _joinableTaskCollection = value.CreateCollection();
                    _joinableTaskFactory = value.CreateFactory(_joinableTaskCollection);
                }
            }
        }

        protected JoinableTaskFactory JoinableTaskFactory => _joinableTaskFactory ?? throw new InvalidOperationException();

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            await _joinableTaskCollection.JoinTillEmptyAsync();
            JoinableTaskContext = null;
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
