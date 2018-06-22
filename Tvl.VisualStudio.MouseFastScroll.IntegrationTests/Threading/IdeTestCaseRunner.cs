// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public sealed class IdeTestCaseRunner : XunitTestCaseRunner
    {
        public IdeTestCaseRunner(
            WpfTestSharedData sharedData,
            VisualStudioVersion visualStudioVersion,
            IXunitTestCase testCase,
            string displayName,
            string skipReason,
            object[] constructorArguments,
            object[] testMethodArguments,
            IMessageBus messageBus,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
            : base(testCase, displayName, skipReason, constructorArguments, testMethodArguments, messageBus, aggregator, cancellationTokenSource)
        {
            SharedData = sharedData;
            VisualStudioVersion = visualStudioVersion;
        }

        public WpfTestSharedData SharedData
        {
            get;
        }

        public VisualStudioVersion VisualStudioVersion
        {
            get;
        }

        protected override XunitTestRunner CreateTestRunner(ITest test, IMessageBus messageBus, Type testClass, object[] constructorArguments, MethodInfo testMethod, object[] testMethodArguments, string skipReason, IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var runner = new IdeTestRunner(SharedData, VisualStudioVersion, test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource);
            return runner;
        }
    }
}
