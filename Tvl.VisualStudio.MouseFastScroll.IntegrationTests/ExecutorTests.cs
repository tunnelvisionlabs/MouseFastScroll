namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using System.IO;
    using EnvDTE;
    using Xunit;
    using Xunit.Abstractions;

    public abstract class ExecutorTests : AbstractIntegrationTest
    {
        protected ExecutorTests(VisualStudioInstanceFactory instanceFactory, Version version)
            : base(instanceFactory, version)
        {
        }

        [VersionTrait(typeof(VS2017))]
        public sealed class VS2017 : ExecutorTests
        {
            ITestOutputHelper _output;

            public VS2017(VisualStudioInstanceFactory instanceFactory, ITestOutputHelper output)
                : base(instanceFactory, Versions.VisualStudio2015)
            {
                _output = output;
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

            [Fact]
            public void DumpWindowNames()
            {
                var captions = VisualStudio.ExecuteInHostProcess((DTE dte) =>
                {
                    var writer = new StringWriter();
                    foreach (Window window in dte.Windows)
                    {
                        writer.WriteLine(window.Caption);
                    }

                    return writer.ToString();
                });

                _output.WriteLine(captions);
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
