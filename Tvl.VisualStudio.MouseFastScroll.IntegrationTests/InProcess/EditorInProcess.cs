// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Microsoft.VisualStudio.Extensibility.Testing
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Formatting;
    using Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess;

    internal partial class EditorInProcess
    {
        public async Task<string> GetTextAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            return view.TextSnapshot.GetText();
        }

        public async Task SetTextAsync(string text, CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            var textSnapshot = view.TextSnapshot;
            var replacementSpan = new SnapshotSpan(textSnapshot, 0, textSnapshot.Length);
            view.TextBuffer.Replace(replacementSpan, text);
        }

        public async Task ActivateAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var dte = await GetRequiredGlobalServiceAsync<SDTE, EnvDTE.DTE>(cancellationToken);
            dte.ActiveDocument.Activate();
        }

        public async Task<int> GetCaretPositionAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);

            var subjectBuffer = view.GetBufferContainingCaret();
            Assumes.Present(subjectBuffer);

            var bufferPosition = view.Caret.Position.BufferPosition;
            return bufferPosition.Position;
        }

        public async Task MoveCaretAsync(int position, CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            var subjectBuffer = view.GetBufferContainingCaret();
            var point = new SnapshotPoint(subjectBuffer.CurrentSnapshot, position);

            view.Caret.MoveTo(point);
        }

        public async Task<bool> IsCaretOnScreenAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            var caret = view.Caret;

            return caret.Left >= view.ViewportLeft
                && caret.Right <= view.ViewportRight
                && caret.Top >= view.ViewportTop
                && caret.Bottom <= view.ViewportBottom;
        }

        public async Task<int> GetFirstVisibleLineAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            return view.TextViewLines.FirstVisibleLine.Start.GetContainingLine().LineNumber;
        }

        public async Task<int> GetLastVisibleLineAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            return view.TextViewLines.LastVisibleLine.Start.GetContainingLine().LineNumber;
        }

        public async Task<VisibilityState> GetLastVisibleLineStateAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            return view.TextViewLines.LastVisibleLine.VisibilityState;
        }

        public async Task<Point> GetCenterOfEditorOnScreenAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            var center = new Point(view.VisualElement.ActualWidth / 2, view.VisualElement.ActualHeight / 2);
            return view.VisualElement.PointToScreen(center);
        }

        public async Task<double> GetZoomLevelAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var view = await GetActiveTextViewAsync(cancellationToken);
            return view.ZoomLevel;
        }
    }
}
