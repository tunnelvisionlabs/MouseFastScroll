// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    internal class IdeTestCollectionRunner : XunitTestCollectionRunner
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public IdeTestCollectionRunner(ITestCollection testCollection, IEnumerable<IXunitTestCase> testCases, IMessageSink diagnosticMessageSink, IMessageBus messageBus, ITestCaseOrderer testCaseOrderer, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
            : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        protected override async Task<RunSummary> RunTestClassesAsync()
        {
            var summary = new RunSummary();

            foreach (var testCasesByTargetVersion in TestCases.GroupBy(GetVisualStudioVersionForTestCase))
            {
                foreach (var testCasesByClass in testCasesByTargetVersion.GroupBy(tc => tc.TestMethod.TestClass, TestClassComparer.Instance))
                {
                    summary.Aggregate(await RunTestClassAsync(testCasesByClass.Key, (IReflectionTypeInfo)testCasesByClass.Key.Class, testCasesByClass));
                    if (CancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }

            return summary;
        }

        private VisualStudioVersion GetVisualStudioVersionForTestCase(IXunitTestCase testCase)
        {
            if (testCase is IdeTestCase ideTestCase)
            {
                return ideTestCase.VisualStudioVersion;
            }

            return VisualStudioVersion.Unspecified;
        }
    }
}
