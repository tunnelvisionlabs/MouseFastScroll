// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading;
    using Xunit;
    using _DTE = EnvDTE._DTE;
    using DTE = EnvDTE.DTE;
    using ServiceProvider = Microsoft.VisualStudio.Shell.ServiceProvider;
    using ThreadHelper = Microsoft.VisualStudio.Shell.ThreadHelper;

    [Collection(nameof(SharedIntegrationHostFixture))]
    public class IdeFactTest
    {
        public IdeFactTest(VisualStudioInstanceFactory instanceFactory)
        {
        }

        [IdeFact(MinVersion = VisualStudioVersion.VS2013)]
        public void TestOpenAndCloseIDE()
        {
            var dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(_DTE));
            var currentVersion = dte.Version;
            Assert.Equal(15, new Version(currentVersion).Major);
        }

        [IdeFact(MaxVersion = VisualStudioVersion.VS2015)]
        public void TestRunsOnUIThread()
        {
            Assert.True(ThreadHelper.CheckAccess());
        }

        [IdeFact(MinVersion = VisualStudioVersion.VS2013, MaxVersion = VisualStudioVersion.VS2015)]
        public async Task TestRunsOnUIThreadAsync()
        {
            Assert.True(ThreadHelper.CheckAccess());
            await Task.Yield();
            Assert.True(ThreadHelper.CheckAccess());
        }

        [IdeFact]
        public async Task TestYieldsToWorkAsync()
        {
            Assert.True(ThreadHelper.CheckAccess());
            await Task.Factory.StartNew(
                () => { },
                CancellationToken.None,
                TaskCreationOptions.None,
                new SynchronizationContextTaskScheduler(new DispatcherSynchronizationContext(Application.Current.Dispatcher)));
            Assert.True(ThreadHelper.CheckAccess());
        }
    }
}
