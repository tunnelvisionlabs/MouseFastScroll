// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness
{
    using System;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        private const string User32 = "User32.dll";

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        [DllImport(User32, CharSet = CharSet.Unicode)]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport(User32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT point);

        public static System.Windows.Point GetCursorPos()
        {
            if (!GetCursorPos(out var point))
            {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            return new System.Windows.Point(point.x.ToInt64(), point.y.ToInt64());
        }

        public struct POINT
        {
            public IntPtr x;
            public IntPtr y;
        }
    }
}
