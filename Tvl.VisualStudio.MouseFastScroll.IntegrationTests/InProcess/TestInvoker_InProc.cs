// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Threading;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    internal class TestInvoker_InProc : InProcComponent
    {
        private TestInvoker_InProc()
        {
            AppDomain.CurrentDomain.AssemblyResolve += VisualStudioInstanceFactory.AssemblyResolveHandler;
        }

        public static TestInvoker_InProc Create()
            => new TestInvoker_InProc();

        public void LoadAssembly(string codeBase)
        {
            var assembly = Assembly.LoadFrom(codeBase);
        }

        public Tuple<decimal, Exception> InvokeTest(
            ITest test,
            IMessageBus messageBus,
            Type testClass,
            object[] constructorArguments,
            MethodInfo testMethod,
            object[] testMethodArguments)
        {
            var aggregator = new ExceptionAggregator();
            var beforeAfterAttributes = new BeforeAfterTestAttribute[0];
            var cancellationTokenSource = new CancellationTokenSource();

            var synchronizationContext = new DispatcherSynchronizationContext(Application.Current.Dispatcher, DispatcherPriority.Background);
            var result = Task.Factory.StartNew(
                async () =>
                {
                    try
                    {
                        var invoker = new XunitTestInvoker(
                            test,
                            messageBus,
                            testClass,
                            constructorArguments,
                            testMethod,
                            testMethodArguments,
                            beforeAfterAttributes,
                            aggregator,
                            cancellationTokenSource);
                        return await invoker.RunAsync();
                    }
                    catch (Exception ex)
                    {
                        Debugger.Launch();
                        throw;
                    }
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                new SynchronizationContextTaskScheduler(synchronizationContext)).Unwrap().GetAwaiter().GetResult();

            return Tuple.Create(result, aggregator.ToException());
        }
    }
}
