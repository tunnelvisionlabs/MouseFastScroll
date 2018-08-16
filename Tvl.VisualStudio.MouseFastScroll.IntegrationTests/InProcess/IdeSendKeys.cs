// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Threading;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess;
    using WindowsInput;
    using WindowsInput.Native;

    public class IdeSendKeys
    {
        private readonly VisualStudio_InProc2 _visualStudio;

        public IdeSendKeys(JoinableTaskFactory joinableTaskFactory)
        {
            _visualStudio = new VisualStudio_InProc2(joinableTaskFactory);
        }

        internal async Task SendAsync(params object[] keys)
        {
            await SendAsync(inputSimulator =>
            {
                foreach (var key in keys)
                {
                    switch (key)
                    {
                    case string str:
                        var text = str.Replace("\r\n", "\r").Replace("\n", "\r");
                        int index = 0;
                        while (index < text.Length)
                        {
                            if (text[index] == '\r')
                            {
                                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                                index++;
                            }
                            else
                            {
                                int nextIndex = text.IndexOf('\r', index);
                                if (nextIndex == -1)
                                {
                                    nextIndex = text.Length;
                                }

                                inputSimulator.Keyboard.TextEntry(text.Substring(index, nextIndex - index));
                                index = nextIndex;
                            }
                        }

                        break;

                    case char c:
                        inputSimulator.Keyboard.TextEntry(c);
                        break;

                    case VirtualKeyCode virtualKeyCode:
                        inputSimulator.Keyboard.KeyPress(virtualKeyCode);
                        break;

                    case null:
                        throw new ArgumentNullException(nameof(keys));

                    default:
                        throw new ArgumentException($"Unexpected type encountered: {key.GetType()}", nameof(keys));
                    }
                }
            });
        }

        internal async Task SendAsync(Action<InputSimulator> actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }

            var foregroundWindow = IntPtr.Zero;

            try
            {
                var foreground = GetForegroundWindow();
                await _visualStudio.ActivateMainWindowAsync();

                await Task.Run(() => actions(new InputSimulator()));
            }
            finally
            {
                if (foregroundWindow != IntPtr.Zero)
                {
                    SetForegroundWindow(foregroundWindow);
                }
            }

            await InProcComponent2.WaitForApplicationIdleAsync();
        }

        private static bool AttachThreadInput(uint idAttach, uint idAttachTo)
        {
            var success = NativeMethods.AttachThreadInput(idAttach, idAttachTo, true);
            if (!success)
            {
                var hresult = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(hresult);
            }

            return success;
        }

        private static bool DetachThreadInput(uint idAttach, uint idAttachTo)
        {
            var success = NativeMethods.AttachThreadInput(idAttach, idAttachTo, false);
            if (!success)
            {
                var hresult = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(hresult);
            }

            return success;
        }

        private static IntPtr GetForegroundWindow()
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

        private static void SetForegroundWindow(IntPtr window, bool skipAttachingThread = false)
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
                if (!skipAttachingThread)
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
