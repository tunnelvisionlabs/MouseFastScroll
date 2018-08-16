// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Threading;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;

    internal partial class VisualStudio_InProc2 : InProcComponent2
    {
        public VisualStudio_InProc2(JoinableTaskFactory joinableTaskFactory)
            : base(joinableTaskFactory)
        {
        }

        public async Task ActivateMainWindowAsync()
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = await GetDTEAsync();

            var activeVisualStudioWindow = (IntPtr)dte.ActiveWindow.HWnd;
            Debug.WriteLine($"DTE.ActiveWindow.HWnd = {activeVisualStudioWindow}");

            if (activeVisualStudioWindow == IntPtr.Zero)
            {
                activeVisualStudioWindow = (IntPtr)dte.MainWindow.HWnd;
                Debug.WriteLine($"DTE.MainWindow.HWnd = {activeVisualStudioWindow}");
            }

            IntegrationHelper.SetForegroundWindow(activeVisualStudioWindow);
        }
    }
}
