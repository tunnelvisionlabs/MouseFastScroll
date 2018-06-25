// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Windows;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Formatting;
    using Microsoft.VisualStudio.TextManager.Interop;
    using Microsoft.VisualStudio.Threading;

    internal class Editor_InProc2 : TextViewWindow_InProc2
    {
        private static readonly Guid IWpfTextViewId = new Guid("8C40265E-9FDB-4F54-A0FD-EBB72B7D0476");

        public Editor_InProc2(JoinableTaskFactory joinableTaskFactory)
            : base(joinableTaskFactory)
        {
        }

        protected override async Task<IWpfTextView> GetActiveTextViewAsync()
        {
            return (await GetActiveTextViewHostAsync()).TextView;
        }

        private async Task<IVsTextView> GetActiveVsTextViewAsync()
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            var vsTextManager = await GetGlobalServiceAsync<SVsTextManager, IVsTextManager>();

            var hresult = vsTextManager.GetActiveView(fMustHaveFocus: 1, pBuffer: null, ppView: out var vsTextView);
            Marshal.ThrowExceptionForHR(hresult);

            return vsTextView;
        }

        private async Task<IWpfTextViewHost> GetActiveTextViewHostAsync()
        {
            // The active text view might not have finished composing yet, waiting for the application to 'idle'
            // means that it is done pumping messages (including WM_PAINT) and the window should return the correct text view
            await WaitForApplicationIdleAsync();

            await JoinableTaskFactory.SwitchToMainThreadAsync();

            var activeVsTextView = (IVsUserData)await GetActiveVsTextViewAsync();

            var hresult = activeVsTextView.GetData(IWpfTextViewId, out var wpfTextViewHost);
            Marshal.ThrowExceptionForHR(hresult);

            return (IWpfTextViewHost)wpfTextViewHost;
        }

        public async Task ActivateAsync()
        {
            (await GetDTEAsync()).ActiveDocument.Activate();
        }

        public async Task<string> GetTextAsync()
        {
            return await ExecuteOnActiveViewAsync(view => Task.FromResult(view.TextSnapshot.GetText()));
        }

        public async Task SetTextAsync(string text)
        {
            await ExecuteOnActiveViewAsync(
                view =>
                {
                    var textSnapshot = view.TextSnapshot;
                    var replacementSpan = new SnapshotSpan(textSnapshot, 0, textSnapshot.Length);
                    view.TextBuffer.Replace(replacementSpan, text);
                    return Task.CompletedTask;
                });
        }

        public async Task MoveCaretAsync(int position)
        {
            await ExecuteOnActiveViewAsync(
                view =>
                {
                    var subjectBuffer = view.GetBufferContainingCaret();
                    var point = new SnapshotPoint(subjectBuffer.CurrentSnapshot, position);

                    view.Caret.MoveTo(point);

                    return Task.CompletedTask;
                });
        }

        public async Task<bool> IsCaretOnScreenAsync()
        {
            return await ExecuteOnActiveViewAsync(
                view =>
                {
                    var caret = view.Caret;
                    return Task.FromResult(caret.Left >= view.ViewportLeft
                        && caret.Right <= view.ViewportRight
                        && caret.Top >= view.ViewportTop
                        && caret.Bottom <= view.ViewportBottom);
                });
        }

        protected override Task<ITextBuffer> GetBufferContainingCaretAsync(IWpfTextView view)
        {
            return Task.FromResult(view.GetBufferContainingCaret());
        }

        public async Task<int> GetFirstVisibleLineAsync()
        {
            return await ExecuteOnActiveViewAsync(view => Task.FromResult(view.TextViewLines.FirstVisibleLine.Start.GetContainingLine().LineNumber));
        }

        public async Task<int> GetLastVisibleLineAsync()
        {
            return await ExecuteOnActiveViewAsync(view => Task.FromResult(view.TextViewLines.LastVisibleLine.Start.GetContainingLine().LineNumber));
        }

        public async Task<VisibilityState> GetLastVisibleLineStateAsync()
        {
            return await ExecuteOnActiveViewAsync(view => Task.FromResult(view.TextViewLines.LastVisibleLine.VisibilityState));
        }

        public async Task<Point> GetCenterOfEditorOnScreenAsync()
        {
            return await ExecuteOnActiveViewAsync(
                view =>
                {
                    var center = new Point(view.VisualElement.ActualWidth / 2, view.VisualElement.ActualHeight / 2);
                    return Task.FromResult(view.VisualElement.PointToScreen(center));
                });
        }

        public async Task<double> GetZoomLevelAsync()
            => await ExecuteOnActiveViewAsync(view => Task.FromResult(view.ZoomLevel));
    }
}
