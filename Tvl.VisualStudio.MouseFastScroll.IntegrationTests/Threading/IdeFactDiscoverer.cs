// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using System.Collections.Generic;
    using System.Linq;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class IdeFactDiscoverer : IXunitTestCaseDiscoverer
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public IdeFactDiscoverer(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
        }

        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo factAttribute)
        {
            if (!testMethod.Method.GetParameters().Any())
            {
                if (!testMethod.Method.IsGenericMethodDefinition)
                {
                    var testCases = new List<IXunitTestCase>();
                    foreach (var supportedVersion in GetSupportedVersions(factAttribute))
                    {
                        yield return new IdeTestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, supportedVersion);
                    }
                }
                else
                {
                    yield return new ExecutionErrorTestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, "[Fact] methods are not allowed to be generic.");
                }
            }
            else
            {
                yield return new ExecutionErrorTestCase(_diagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(), testMethod, "[Fact] methods are not allowed to have parameters. Did you mean to use [Theory]?");
            }
        }

        private IEnumerable<VisualStudioVersion> GetSupportedVersions(IAttributeInfo theoryAttribute)
        {
            var minVersion = theoryAttribute.GetNamedArgument<VisualStudioVersion>(nameof(IdeFactAttribute.MinVersion));
            minVersion = minVersion == VisualStudioVersion.Unspecified ? VisualStudioVersion.VS2012 : minVersion;

            var maxVersion = theoryAttribute.GetNamedArgument<VisualStudioVersion>(nameof(IdeFactAttribute.MaxVersion));
            maxVersion = maxVersion == VisualStudioVersion.Unspecified ? VisualStudioVersion.VS2017 : maxVersion;

            for (var version = minVersion; version <= maxVersion; version++)
            {
                yield return version;
            }
        }
    }
}
