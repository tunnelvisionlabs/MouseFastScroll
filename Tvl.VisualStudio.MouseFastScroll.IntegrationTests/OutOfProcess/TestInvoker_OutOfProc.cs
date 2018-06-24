// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.OutOfProcess
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.Harness;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess;
    using Xunit.Abstractions;
    using Xunit.Sdk;

    public class TestInvoker_OutOfProc : OutOfProcComponent
    {
        internal TestInvoker_OutOfProc(VisualStudioInstance visualStudioInstance)
            : base(visualStudioInstance)
        {
            TestInvokerInProc = CreateInProcComponent<TestInvoker_InProc>(visualStudioInstance);
        }

        internal TestInvoker_InProc TestInvokerInProc
        {
            get;
        }

        public void LoadAssembly(string codeBase)
        {
            TestInvokerInProc.LoadAssembly(codeBase);
        }

        public InProcessIdeTestAssemblyRunner CreateTestAssemblyRunner(ITestAssembly testAssembly, IXunitTestCase[] testCases, IMessageSink diagnosticMessageSink, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            return TestInvokerInProc.CreateTestAssemblyRunner(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions);
        }

        public Tuple<decimal, Exception> InvokeTest(
            ITest test,
            IMessageBus messageBus,
            Type testClass,
            object[] constructorArguments,
            MethodInfo testMethod,
            object[] testMethodArguments)
        {
            if (constructorArguments != null)
            {
                if (constructorArguments.OfType<ITestOutputHelper>().Any())
                {
                    constructorArguments = (object[])constructorArguments.Clone();
                    for (int i = 0; i < constructorArguments.Length; i++)
                    {
                        if (constructorArguments[i] is ITestOutputHelper testOutputHelper)
                        {
                            constructorArguments[i] = new TestOutputHelperWrapper(testOutputHelper);
                        }
                    }
                }
            }

            return TestInvokerInProc.InvokeTest(
                test,
                messageBus,
                testClass,
                constructorArguments,
                testMethod,
                testMethodArguments);
        }

        private class TestOutputHelperWrapper : MarshalByRefObject, ITestOutputHelper
        {
            private readonly ITestOutputHelper _testOutputHelper;

            public TestOutputHelperWrapper(ITestOutputHelper testOutputHelper)
            {
                _testOutputHelper = testOutputHelper;
            }

            public void WriteLine(string message)
            {
                _testOutputHelper.WriteLine(message);
            }

            public void WriteLine(string format, params object[] args)
            {
                _testOutputHelper.WriteLine(format, args);
            }
        }
    }
}
