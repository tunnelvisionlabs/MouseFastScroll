// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.Threading;
    using Xunit;
    using vsSaveChanges = EnvDTE.vsSaveChanges;

    public abstract class TrivialIntegrationTest : AbstractIntegrationTest
    {
        protected TrivialIntegrationTest(VisualStudioInstanceFactory instanceFactory, Version version)
            : base(instanceFactory, version)
        {
        }

        [Fact]
        public void TestOpenAndCloseIDE()
        {
            var currentVersion = VisualStudioInstance.RetryRpcCall(() => VisualStudio.Dte.Version);
            var expectedVersion = VisualStudio.Version;
            if (expectedVersion.Major >= 15)
            {
                // The Setup APIs provide the full version number, but DTE.Version is always [Major].0
                expectedVersion = new Version(expectedVersion.Major, 0);
            }

            Assert.Equal(expectedVersion.ToString(), currentVersion);
        }

        [Fact]
        public void OpenDocumentAndType()
        {
            var window = VisualStudioInstance.RetryRpcCall(() => VisualStudio.Dte.ItemOperations.NewFile(Name: Guid.NewGuid() + ".txt"));
            VisualStudio.Editor.SendKeys(Guid.NewGuid().ToString() + "\n" + Guid.NewGuid().ToString());
            VisualStudioInstance.RetryRpcCall(() => window.Close(vsSaveChanges.vsSaveChangesNo));
        }

        [VersionTrait(typeof(VS2012))]
        public sealed class VS2012 : TrivialIntegrationTest
        {
            public VS2012(VisualStudioInstanceFactory instanceFactory)
                : base(instanceFactory, Versions.VisualStudio2012)
            {
            }
        }

        [VersionTrait(typeof(VS2013))]
        public sealed class VS2013 : TrivialIntegrationTest
        {
            public VS2013(VisualStudioInstanceFactory instanceFactory)
                : base(instanceFactory, Versions.VisualStudio2013)
            {
            }
        }

        [VersionTrait(typeof(VS2015))]
        public sealed class VS2015 : TrivialIntegrationTest
        {
            public VS2015(VisualStudioInstanceFactory instanceFactory)
                : base(instanceFactory, Versions.VisualStudio2015)
            {
            }
        }

        [VersionTrait(typeof(VS2017))]
        public sealed class VS2017 : TrivialIntegrationTest
        {
            public VS2017(VisualStudioInstanceFactory instanceFactory)
                : base(instanceFactory, Versions.VisualStudio2017)
            {
            }
        }
    }
}
