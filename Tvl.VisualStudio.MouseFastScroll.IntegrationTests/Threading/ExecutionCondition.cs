// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    public abstract class ExecutionCondition
    {
        public abstract bool ShouldSkip { get; }

        public abstract string SkipReason { get; }
    }
}
