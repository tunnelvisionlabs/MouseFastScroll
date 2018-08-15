// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Threading;

    internal abstract class TextViewWindow_InProc2 : InProcComponent2
    {
        protected TextViewWindow_InProc2(JoinableTaskFactory joinableTaskFactory)
            : base(joinableTaskFactory)
        {
        }

        protected abstract Task<IWpfTextView> GetActiveTextViewAsync();

        protected abstract Task<ITextBuffer> GetBufferContainingCaretAsync(IWpfTextView view);

        public async Task<int> GetCaretPositionAsync()
        {
            return await ExecuteOnActiveViewAsync(
                async view =>
                {
                    var subjectBuffer = await GetBufferContainingCaretAsync(view);
                    var bufferPosition = view.Caret.Position.BufferPosition;
                    return bufferPosition.Position;
                });
        }

        protected async Task<T> ExecuteOnActiveViewAsync<T>(Func<IWpfTextView, Task<T>> action)
        {
            return await InvokeOnUIThreadAsync(
                async () =>
                {
                    var view = await GetActiveTextViewAsync();
                    return await action(view);
                });
        }

        protected async Task ExecuteOnActiveViewAsync(Func<IWpfTextView, Task> action)
            => await InvokeOnUIThreadAsync(GetExecuteOnActionViewCallback(action));

        protected Func<Task> GetExecuteOnActionViewCallback(Func<IWpfTextView, Task> action)
        {
            return async () =>
            {
                var view = await GetActiveTextViewAsync();
                await action(view);
            };
        }
    }
}
