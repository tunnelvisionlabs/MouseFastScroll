// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using DTE = EnvDTE.DTE;
    using SDTE = Microsoft.VisualStudio.Shell.Interop.SDTE;
    using ServiceProvider = Microsoft.VisualStudio.Shell.ServiceProvider;

    /// <summary>
    /// Base class for all components that run inside of the Visual Studio process.
    /// <list type="bullet">
    /// <item>Every in-proc component should provide a public, static, parameterless "Create" method.
    /// This will be called to construct the component in the VS process.</item>
    /// <item>Public methods on in-proc components should be instance methods to ensure that they are
    /// marshalled properly and execute in the VS process. Static methods will execute in the process
    /// in which they are called.</item>
    /// </list>
    /// </summary>
    internal abstract class InProcComponent : MarshalByRefObject
    {
        protected InProcComponent()
        {
        }

        private static Dispatcher CurrentApplicationDispatcher
            => Application.Current.Dispatcher;

        protected static void InvokeOnUIThread(Action action)
            => CurrentApplicationDispatcher.Invoke(action, DispatcherPriority.Background);

        protected static T InvokeOnUIThread<T>(Func<T> action)
            => CurrentApplicationDispatcher.Invoke(action, DispatcherPriority.Background);

        protected static TInterface GetGlobalService<TService, TInterface>()
            where TService : class
            where TInterface : class
        => InvokeOnUIThread(() => (TInterface)ServiceProvider.GlobalProvider.GetService(typeof(TService)));

        protected static DTE GetDTE()
            => GetGlobalService<SDTE, DTE>();

        /// <summary>
        /// Waiting for the application to 'idle' means that it is done pumping messages (including WM_PAINT).
        /// </summary>
        protected static void WaitForApplicationIdle()
            => CurrentApplicationDispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
    }
}
