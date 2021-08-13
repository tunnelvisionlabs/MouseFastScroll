// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Threading;
    using Xunit;
    using _DTE = EnvDTE._DTE;
    using DTE = EnvDTE.DTE;
    using ServiceProvider = Microsoft.VisualStudio.Shell.ServiceProvider;

    public class IdeFactTest : AbstractIdeIntegrationTest
    {
        [IdeFact]
        public void TestOpenAndCloseIDE()
        {
            Assert.Equal("devenv", Process.GetCurrentProcess().ProcessName);
            var dte = (DTE)ServiceProvider.GetService(typeof(_DTE));
            Assert.NotNull(dte);
        }

        [IdeFact]
        public void TestRunsOnUIThread()
        {
            Assert.True(Application.Current.Dispatcher.CheckAccess());
        }

        [IdeFact]
        public async Task TestRunsOnUIThreadAsync()
        {
            Assert.True(Application.Current.Dispatcher.CheckAccess());
            await Task.Yield();
            Assert.True(Application.Current.Dispatcher.CheckAccess());
        }

        [IdeFact]
        public async Task TestYieldsToWorkAsync()
        {
            Assert.True(Application.Current.Dispatcher.CheckAccess());

            var task = Application.Current.Dispatcher.InvokeAsync(() => { }).Task;
            Assert.False(task.IsCompleted);
            await task;

            Assert.True(Application.Current.Dispatcher.CheckAccess());
        }

        [IdeFact]
        public async Task TestJoinableTaskFactoryAsync()
        {
            Assert.NotNull(JoinableTaskContext);
            Assert.NotNull(JoinableTaskFactory);
            Assert.Equal(Thread.CurrentThread, JoinableTaskContext.MainThread);

            await TaskScheduler.Default;

            Assert.NotEqual(Thread.CurrentThread, JoinableTaskContext.MainThread);

            await JoinableTaskFactory.SwitchToMainThreadAsync();

            Assert.Equal(Thread.CurrentThread, JoinableTaskContext.MainThread);
        }

        [IdeFact(MinVersion = VisualStudioVersion.VS2012, MaxVersion = VisualStudioVersion.VS2012)]
        public void TestJoinableTaskFactoryProvidedByTest()
        {
            var taskSchedulerServiceObject = ServiceProvider.GetService(typeof(SVsTaskSchedulerService));
            Assert.NotNull(taskSchedulerServiceObject);

            var taskSchedulerService = taskSchedulerServiceObject as IVsTaskSchedulerService;
            Assert.NotNull(taskSchedulerService);

            var taskSchedulerService2 = taskSchedulerServiceObject as IVsTaskSchedulerService2;
            Assert.Null(taskSchedulerService2);

            Assert.NotNull(JoinableTaskContext);
        }

        [IdeFact(MinVersion = VisualStudioVersion.VS2013)]
        public void TestJoinableTaskFactoryObtainedFromEnvironment()
        {
            var taskSchedulerServiceObject = ServiceProvider.GetService(typeof(SVsTaskSchedulerService));
            Assert.NotNull(taskSchedulerServiceObject);

            var taskSchedulerService = taskSchedulerServiceObject as IVsTaskSchedulerService2;
            Assert.NotNull(taskSchedulerService);

            Assert.Same(JoinableTaskContext, taskSchedulerService.GetAsyncTaskContext());
        }
    }
}
