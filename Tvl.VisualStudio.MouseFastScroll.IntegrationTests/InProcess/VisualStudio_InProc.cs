// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess
{
    using System;
    using System.Diagnostics;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;

    internal partial class VisualStudio_InProc : InProcComponent
    {
        private VisualStudio_InProc()
        {
        }

        public static VisualStudio_InProc Create()
            => new VisualStudio_InProc();

        public new void WaitForApplicationIdle()
            => InProcComponent.WaitForApplicationIdle();

        public void ActivateMainWindow()
            => InvokeOnUIThread(() =>
            {
                var dte = GetDTE();

                var activeVisualStudioWindow = (IntPtr)dte.ActiveWindow.HWnd;
                Debug.WriteLine($"DTE.ActiveWindow.HWnd = {activeVisualStudioWindow}");

                if (activeVisualStudioWindow == IntPtr.Zero)
                {
                    activeVisualStudioWindow = (IntPtr)dte.MainWindow.HWnd;
                    Debug.WriteLine($"DTE.MainWindow.HWnd = {activeVisualStudioWindow}");
                }

                IntegrationHelper.SetForegroundWindow(activeVisualStudioWindow);
            });
    }
}
