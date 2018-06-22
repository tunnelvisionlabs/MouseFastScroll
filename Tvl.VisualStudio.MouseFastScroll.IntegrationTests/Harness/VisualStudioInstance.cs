// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness
{
    using System;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.OutOfProcess;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTestService;
    using DTE = EnvDTE.DTE;

    public class VisualStudioInstance
    {
        private readonly IntegrationService _integrationService;
        private readonly IpcClientChannel _integrationServiceChannel;
        private readonly VisualStudio_InProc _inProc;

        public VisualStudioInstance(Process hostProcess, DTE dte, Version version, ImmutableHashSet<string> supportedPackageIds, string installationPath)
        {
            HostProcess = hostProcess;
            Dte = dte;
            Version = version;
            SupportedPackageIds = supportedPackageIds;
            InstallationPath = installationPath;

            StartRemoteIntegrationService(dte);

            _integrationServiceChannel = new IpcClientChannel($"IPC channel client for {HostProcess.Id}", sinkProvider: null);
            ChannelServices.RegisterChannel(_integrationServiceChannel, ensureSecurity: true);

            // Connect to a 'well defined, shouldn't conflict' IPC channel
            _integrationService = IntegrationService.GetInstanceFromHostProcess(hostProcess);

            // Create marshal-by-ref object that runs in host-process.
            _inProc = ExecuteInHostProcess<VisualStudio_InProc>(
                type: typeof(VisualStudio_InProc),
                methodName: nameof(VisualStudio_InProc.Create));

            // There is a lot of VS initialization code that goes on, so we want to wait for that to 'settle' before
            // we start executing any actual code.
            _inProc.WaitForSystemIdle();

            SendKeys = new SendKeys(this);
            Editor = new Editor_OutOfProc(this);
            TestInvoker = new TestInvoker_OutOfProc(this);

            // Ensure we are in a known 'good' state by cleaning up anything changed by the previous instance
            CleanUp();
        }

        internal DTE Dte
        {
            get;
        }

        internal Process HostProcess
        {
            get;
        }

        public Version Version
        {
            get;
        }

        /// <summary>
        /// Gets the set of Visual Studio packages that are installed into this instance.
        /// </summary>
        public ImmutableHashSet<string> SupportedPackageIds
        {
            get;
        }

        /// <summary>
        /// Gets the path to the root of this installed version of Visual Studio. This is the folder that contains
        /// Common7\IDE.
        /// </summary>
        public string InstallationPath
        {
            get;
        }

        public SendKeys SendKeys
        {
            get;
        }

        public Editor_OutOfProc Editor
        {
            get;
        }

        public TestInvoker_OutOfProc TestInvoker
        {
            get;
        }

        public int ErrorListErrorCount
            => _inProc.GetErrorListErrorCount();

        public bool IsRunning => !HostProcess.HasExited;

        public void ExecuteInHostProcess(Type type, string methodName)
        {
            var result = _integrationService.Execute(type.Assembly.Location, type.FullName, methodName);

            if (result != null)
            {
                throw new InvalidOperationException("The specified call was not expected to return a value.");
            }
        }

        public T ExecuteInHostProcess<T>(Type type, string methodName)
        {
            var objectUri = _integrationService.Execute(type.Assembly.Location, type.FullName, methodName) ?? throw new InvalidOperationException("The specified call was expected to return a value.");
            return (T)Activator.GetObject(typeof(T), $"{_integrationService.BaseUri}/{objectUri}");
        }

        public void ActivateMainWindow(bool skipAttachingThreads = false)
            => _inProc.ActivateMainWindow(skipAttachingThreads);

        public void WaitForApplicationIdle()
            => _inProc.WaitForApplicationIdle();

        public void ExecuteCommand(string commandName, string argument = "")
            => _inProc.ExecuteCommand(commandName, argument);

        public bool IsCommandAvailable(string commandName)
            => _inProc.IsCommandAvailable(commandName);

        public string[] GetAvailableCommands()
            => _inProc.GetAvailableCommands();

        public void WaitForNoErrorsInErrorList()
            => _inProc.WaitForNoErrorsInErrorList();

        public void CleanUp()
        {
        }

        public void Close(bool exitHostProcess = true)
        {
            if (!IsRunning)
            {
                return;
            }

            CleanUp();

            CloseRemotingService();

            if (exitHostProcess)
            {
                CloseHostProcess();
            }
        }

        private void CloseHostProcess()
        {
            _inProc.Quit();
            IntegrationHelper.KillProcess(HostProcess);
        }

        private void CloseRemotingService()
        {
            try
            {
                StopRemoteIntegrationService();
            }
            finally
            {
                if (_integrationServiceChannel != null
                    && ChannelServices.RegisteredChannels.Contains(_integrationServiceChannel))
                {
                    ChannelServices.UnregisterChannel(_integrationServiceChannel);
                }
            }
        }

        private void StartRemoteIntegrationService(DTE dte)
        {
            // We use DTE over RPC to start the integration service. All other DTE calls should happen in the host process.
            if (dte.Commands.Item(WellKnownCommandNames.IntegrationTestServiceStart).IsAvailable)
            {
                dte.ExecuteCommand(WellKnownCommandNames.IntegrationTestServiceStart);
            }
        }

        private void StopRemoteIntegrationService()
        {
            if (_inProc.IsCommandAvailable(WellKnownCommandNames.IntegrationTestServiceStop))
            {
                _inProc.ExecuteCommand(WellKnownCommandNames.IntegrationTestServiceStop);
            }
        }
    }
}
