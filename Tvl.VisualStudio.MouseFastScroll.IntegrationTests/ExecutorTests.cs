namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.IO;
    using EnvDTE;
    using Xunit;

    public abstract class ExecutorTests : AbstractIntegrationTest
    {
        protected ExecutorTests(VisualStudioInstanceFactory instanceFactory, Version version)
            : base(instanceFactory, version)
        {
        }

        [VersionTrait(typeof(VS2015))]
        public sealed class VS2015 : ExecutorTests
        {
            public VS2015(VisualStudioInstanceFactory instanceFactory)
                : base(instanceFactory, Versions.VisualStudio2015)
            {
            }

            [Fact]
            public void ReturnLocal()
            {
                var local = "local";

                var ret = VisualStudio.ExecuteInHostProcess(() => local);

                Assert.Equal(local, ret);
            }

            [Fact]
            public void ReturnLiteral()
            {
                var ret = VisualStudio.ExecuteInHostProcess(() => "literal");

                Assert.Equal("literal", ret);
            }

            [Fact]
            public void ReturnFromDte()
            {
                var file = VisualStudio.ExecuteInHostProcess((DTE dte) => dte.FileName);

                Assert.True(File.Exists(file));
            }
        }
    }
}
