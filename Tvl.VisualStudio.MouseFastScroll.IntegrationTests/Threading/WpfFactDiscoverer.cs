﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class WpfFactDiscoverer : FactDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public WpfFactDiscoverer(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        protected override IXunitTestCase CreateTestCase(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
            => new WpfTestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod);
    }
}