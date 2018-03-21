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
            public void CanUseDTE()
            {
                var version = VisualStudio.ExecuteInHostProcess((DTE dte) => dte.Version);

                Assert.NotNull(version);
            }

            // This is to check Windows exist on the AppVeyor instance!
            [Fact]
            public void WindowsSanityCheck()
            {
                var count = VisualStudio.ExecuteInHostProcess((DTE dte) => dte.Windows.Count);

                Assert.NotEqual(0, count);
            }

            [Theory]
            [InlineData("Output", true)]
            [InlineData("XXX", false)]
            public void CanUseMethodArguments(string name, bool exists)
            {
                Assert.Equal(exists, WindowExists(name));
            }

            private bool WindowExists(string name)
            {
                return VisualStudio.ExecuteInHostProcess((DTE dte) =>
                {
                    try
                    {
                        dte.Windows.Item(name);
                        return true;
                    }
                    catch (ArgumentException)
                    {
                    }

                    return false;
                });
            }
        }
    }
}
