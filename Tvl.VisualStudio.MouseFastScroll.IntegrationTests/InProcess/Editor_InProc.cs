// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;

    internal class Editor_InProc : TextViewWindow_InProc
    {
        private static readonly Guid IWpfTextViewId = new Guid("8C40265E-9FDB-4F54-A0FD-EBB72B7D0476");

        private Editor_InProc()
        {
        }

        public static Editor_InProc Create()
            => new Editor_InProc();

        protected override IWpfTextView GetActiveTextView()
            => GetActiveTextViewHost().TextView;

        private static IVsTextView GetActiveVsTextView()
        {
            var vsTextManager = GetGlobalService<SVsTextManager, IVsTextManager>();

            var hresult = vsTextManager.GetActiveView(fMustHaveFocus: 1, pBuffer: null, ppView: out var vsTextView);
            Marshal.ThrowExceptionForHR(hresult);

            return vsTextView;
        }

        private static IWpfTextViewHost GetActiveTextViewHost()
        {
            // The active text view might not have finished composing yet, waiting for the application to 'idle'
            // means that it is done pumping messages (including WM_PAINT) and the window should return the correct text view
            WaitForApplicationIdle();

            var activeVsTextView = (IVsUserData)GetActiveVsTextView();

            var hresult = activeVsTextView.GetData(IWpfTextViewId, out var wpfTextViewHost);
            Marshal.ThrowExceptionForHR(hresult);

            return (IWpfTextViewHost)wpfTextViewHost;
        }

        public void Activate()
            => GetDTE().ActiveDocument.Activate();

        public string GetText()
            => ExecuteOnActiveView(view => view.TextSnapshot.GetText());

        public void SetText(string text)
        {
            ExecuteOnActiveView(
                view =>
                {
                    var textSnapshot = view.TextSnapshot;
                    var replacementSpan = new SnapshotSpan(textSnapshot, 0, textSnapshot.Length);
                    view.TextBuffer.Replace(replacementSpan, text);
                });
        }

        public void MoveCaret(int position)
        {
            ExecuteOnActiveView(
                view =>
                {
                    var subjectBuffer = view.GetBufferContainingCaret();
                    var point = new SnapshotPoint(subjectBuffer.CurrentSnapshot, position);

                    view.Caret.MoveTo(point);
                });
        }

        public bool IsCaretOnScreen()
        {
            return ExecuteOnActiveView(
                view =>
                {
                    var caret = view.Caret;
                    return caret.Left >= view.ViewportLeft
                        && caret.Right <= view.ViewportRight
                        && caret.Top >= view.ViewportTop
                        && caret.Bottom <= view.ViewportBottom;
                });
        }

        protected override ITextBuffer GetBufferContainingCaret(IWpfTextView view)
        {
            return view.GetBufferContainingCaret();
        }

        public int GetFirstVisibleLine()
        {
            return ExecuteOnActiveView(view => view.TextViewLines.FirstVisibleLine.Start.GetContainingLine().LineNumber);
        }

        public int GetLastVisibleLine()
        {
            return ExecuteOnActiveView(view => view.TextViewLines.LastVisibleLine.Start.GetContainingLine().LineNumber);
        }

        public int GetLastVisibleLineState()
        {
            return ExecuteOnActiveView(view => (int)view.TextViewLines.LastVisibleLine.VisibilityState);
        }

        public Point GetCenterOfEditorOnScreen()
        {
            return ExecuteOnActiveView(
                view =>
                {
                    var center = new Point(view.VisualElement.ActualWidth / 2, view.VisualElement.ActualHeight / 2);
                    return view.VisualElement.PointToScreen(center);
                });
        }

        public double GetZoomLevel()
            => ExecuteOnActiveView(view => view.ZoomLevel);
    }
}
