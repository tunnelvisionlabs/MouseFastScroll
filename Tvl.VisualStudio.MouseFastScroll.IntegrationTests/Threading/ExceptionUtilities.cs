// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using System;

    internal static class ExceptionUtilities
    {
        internal static Exception Unreachable
            => new InvalidOperationException("This program location is thought to be unreachable.");
    }
}
