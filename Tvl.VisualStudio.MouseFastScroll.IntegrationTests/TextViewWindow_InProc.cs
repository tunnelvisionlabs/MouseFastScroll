// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System;
    using Microsoft.VisualStudio.Text.Editor;

    internal abstract class TextViewWindow_InProc : InProcComponent
    {
        protected abstract IWpfTextView GetActiveTextView();

        protected T ExecuteOnActiveView<T>(Func<IWpfTextView, T> action)
        {
            return InvokeOnUIThread(
                () =>
                {
                    var view = GetActiveTextView();
                    return action(view);
                });
        }

        protected void ExecuteOnActiveView(Action<IWpfTextView> action)
            => InvokeOnUIThread(GetExecuteOnActionViewCallback(action));

        protected Action GetExecuteOnActionViewCallback(Action<IWpfTextView> action)
        {
            return () =>
            {
                var view = GetActiveTextView();
                action(view);
            };
        }
    }
}
