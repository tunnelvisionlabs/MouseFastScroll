// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Threading.Tasks;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public sealed class IdeTestCase : XunitTestCase
    {
        private VisualStudioVersion _visualStudioVersion;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the deserializer; should only be called by deriving classes for deserialization purposes")]
        public IdeTestCase()
        {
        }

        public IdeTestCase(IMessageSink diagnosticMessageSink, TestMethodDisplay defaultMethodDisplay, ITestMethod testMethod, VisualStudioVersion visualStudioVersion, object[] testMethodArguments = null)
            : base(diagnosticMessageSink, defaultMethodDisplay, testMethod, testMethodArguments)
        {
            SharedData = WpfTestSharedData.Instance;
            _visualStudioVersion = visualStudioVersion;
        }

        public WpfTestSharedData SharedData
        {
            get;
            private set;
        }

        protected override string GetDisplayName(IAttributeInfo factAttribute, string displayName)
        {
            var baseName = base.GetDisplayName(factAttribute, displayName);
            return $"{baseName} ({_visualStudioVersion})";
        }

        protected override string GetUniqueID()
        {
            return $"{base.GetUniqueID()}_{_visualStudioVersion}";
        }

        public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments, ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var runner = new IdeTestCaseRunner(SharedData, _visualStudioVersion, this, DisplayName, SkipReason, constructorArguments, TestMethodArguments, messageBus, aggregator, cancellationTokenSource);
            return runner.RunAsync();
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);
            data.AddValue(nameof(_visualStudioVersion), (int)_visualStudioVersion);
        }

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);
            _visualStudioVersion = (VisualStudioVersion)data.GetValue<int>(nameof(_visualStudioVersion));
            SharedData = WpfTestSharedData.Instance;
        }
    }
}
