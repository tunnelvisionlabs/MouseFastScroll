// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using System.Runtime.Serialization.Formatters;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Automation;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Xunit;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    /// <summary>
    /// This type is actually responsible for spinning up the STA context to run all of the
    /// tests.
    ///
    /// Overriding the <see cref="XunitTestInvoker"/> to setup the STA context is not the correct
    /// approach. That type begins constructing types before RunAsync and hence constructors end up
    /// running on the current thread vs. the STA ones. Just completely wrapping the invocation
    /// here is the best case.
    /// </summary>
    public sealed class IdeTestRunner : WpfTestRunner
    {
        ////private static readonly IpcServerChannel ServiceChannel = new IpcServerChannel(
        ////    name: $"Microsoft.VisualStudio.IntegrationTest.ServiceChannel_{Process.GetCurrentProcess().Id}",
        ////    portName: $"{nameof(IdeTestRunner)}_{{{Process.GetCurrentProcess().Id}}}",
        ////    sinkProvider: new BinaryServerFormatterSinkProvider { TypeFilterLevel = TypeFilterLevel.Full });

        public IdeTestRunner(
            WpfTestSharedData sharedData,
            ITest test,
            IMessageBus messageBus,
            Type testClass,
            object[] constructorArguments,
            MethodInfo testMethod,
            object[] testMethodArguments,
            string skipReason,
            IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
            ExceptionAggregator aggregator,
            CancellationTokenSource cancellationTokenSource)
            : base(sharedData, test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, beforeAfterAttributes, aggregator, cancellationTokenSource)
        {
        }

        protected override async Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
        {
            return await base.InvokeTestMethodAsync(aggregator).ConfigureAwait(true);
        }

        protected override Func<Task<decimal>> CreateTestInvoker(ExceptionAggregator aggregator)
        {
            return async () =>
            {
                Assert.Equal(ApartmentState.STA, Thread.CurrentThread.GetApartmentState());
                var instanceFactory = ConstructorArguments.OfType<VisualStudioInstanceFactory>().Single();

                // Install a COM message filter to handle retry operations when the first attempt fails
                using (var messageFilter = RegisterMessageFilter())
                {
                    Automation.TransactionTimeout = 20000;
                    var version = new Version(15, 0);
                    using (var visualStudioContext = await instanceFactory.GetNewOrUsedInstanceAsync(version, SharedIntegrationHostFixture.RequiredPackageIds).ConfigureAwait(true))
                    {
                        visualStudioContext.Instance.TestInvoker.LoadAssembly(typeof(ITest).Assembly.Location);
                        visualStudioContext.Instance.TestInvoker.LoadAssembly(typeof(Assert).Assembly.Location);
                        visualStudioContext.Instance.TestInvoker.LoadAssembly(typeof(TheoryAttribute).Assembly.Location);
                        visualStudioContext.Instance.TestInvoker.LoadAssembly(typeof(TestClass).Assembly.Location);

                        ////ServiceChannel.StartListening(null);
                        try
                        {
                            ////ChannelServices.RegisterChannel(ServiceChannel, false);
                            Assert.Empty(BeforeAfterAttributes);
                            var result = visualStudioContext.Instance.TestInvoker.InvokeTest(Test, new IpcMessageBus(MessageBus), TestClass, ConstructorArguments, TestMethod, TestMethodArguments);
                            if (result.Item2 != null)
                            {
                                aggregator.Add(result.Item2);
                            }

                            return result.Item1;
                        }
                        finally
                        {
                            ////ChannelServices.UnregisterChannel(ServiceChannel);
                            ////ServiceChannel.StopListening(null);
                        }
                    }
                }
            };
        }

        private AbstractIntegrationTest.MessageFilter RegisterMessageFilter()
            => new AbstractIntegrationTest.MessageFilter();

        private class IpcMessageBus : MarshalByRefObject, IMessageBus
        {
            private readonly IMessageBus _messageBus;

            public IpcMessageBus(IMessageBus messageBus)
            {
                _messageBus = messageBus;
            }

            public void Dispose() => _messageBus.Dispose();

            public bool QueueMessage(IMessageSinkMessage message) => _messageBus.QueueMessage(message);
        }
    }
}
