// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using System;

    public class ConditionalWpfTheoryAttribute : WpfTheoryAttribute
    {
        public ConditionalWpfTheoryAttribute(Type skipCondition)
        {
            var condition = Activator.CreateInstance(skipCondition) as ExecutionCondition;
            if (condition.ShouldSkip)
            {
                Skip = condition.SkipReason;
            }
        }
    }
}
