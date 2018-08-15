// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides some helper functions used by the other classes in the project.
    /// </summary>
    internal static class IntegrationHelper
    {
        public static bool AttachThreadInput(uint idAttach, uint idAttachTo)
        {
            var success = NativeMethods.AttachThreadInput(idAttach, idAttachTo, true);

            if (!success)
            {
                var hresult = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(hresult);
            }

            return success;
        }

        public static bool DetachThreadInput(uint idAttach, uint idAttachTo)
        {
            var success = NativeMethods.AttachThreadInput(idAttach, idAttachTo, false);

            if (!success)
            {
                var hresult = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(hresult);
            }

            return success;
        }

        public static IntPtr GetForegroundWindow()
        {
            // Attempt to get the foreground window in a loop, as the NativeMethods function can return IntPtr.Zero
            // in certain circumstances, such as when a window is losing activation.
            var foregroundWindow = IntPtr.Zero;

            do
            {
                foregroundWindow = NativeMethods.GetForegroundWindow();
            }
            while (foregroundWindow == IntPtr.Zero);

            return foregroundWindow;
        }

        public static void SetForegroundWindow(IntPtr window)
        {
            var foregroundWindow = GetForegroundWindow();

            if (window == foregroundWindow)
            {
                return;
            }

            var activeThreadId = NativeMethods.GetWindowThreadProcessId(foregroundWindow, IntPtr.Zero);
            var currentThreadId = NativeMethods.GetCurrentThreadId();

            var threadInputsAttached = false;

            try
            {
                // No need to re-attach threads in case when VS initializaed an UI thread for a debugged application.
                if (activeThreadId != currentThreadId)
                {
                    // Attach the thread inputs so that 'SetActiveWindow' and 'SetFocus' work
                    threadInputsAttached = AttachThreadInput(currentThreadId, activeThreadId);
                }

                // Make the window a top-most window so it will appear above any existing top-most windows
                NativeMethods.SetWindowPos(window, (IntPtr)NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOMOVE);

                // Move the window into the foreground as it may not have been achieved by the 'SetWindowPos' call
                var success = NativeMethods.SetForegroundWindow(window);

                if (!success)
                {
                    throw new InvalidOperationException("Setting the foreground window failed.");
                }

                // Ensure the window is 'Active' as it may not have been achieved by 'SetForegroundWindow'
                NativeMethods.SetActiveWindow(window);

                // Give the window the keyboard focus as it may not have been achieved by 'SetActiveWindow'
                NativeMethods.SetFocus(window);

                // Remove the 'Top-Most' qualification from the window
                NativeMethods.SetWindowPos(window, (IntPtr)NativeMethods.HWND_NOTOPMOST, 0, 0, 0, 0, NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOMOVE);
            }
            finally
            {
                if (threadInputsAttached)
                {
                    // Finally, detach the thread inputs from eachother
                    DetachThreadInput(currentThreadId, activeThreadId);
                }
            }
        }
    }
}
