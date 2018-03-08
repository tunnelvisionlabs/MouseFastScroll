// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests.Fakes
{
    using System;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;

    internal class FakeViewScroller : IViewScroller
    {
        private readonly FakeWpfTextView _wpfTextView;

        public FakeViewScroller(FakeWpfTextView wpfTextView)
        {
            _wpfTextView = wpfTextView;
        }

        public void EnsureSpanVisible(SnapshotSpan span)
        {
            throw new NotImplementedException();
        }

        public void EnsureSpanVisible(SnapshotSpan span, EnsureSpanVisibleOptions options)
        {
            throw new NotImplementedException();
        }

        public void EnsureSpanVisible(VirtualSnapshotSpan span, EnsureSpanVisibleOptions options)
        {
            throw new NotImplementedException();
        }

        public void ScrollViewportHorizontallyByPixels(double distanceToScroll)
        {
            throw new NotImplementedException();
        }

        public void ScrollViewportVerticallyByLine(ScrollDirection direction)
        {
            ScrollViewportVerticallyByLines(direction, 1);
        }

        public void ScrollViewportVerticallyByLines(ScrollDirection direction, int count)
        {
            _wpfTextView.TextViewLines.Scroll(direction, count);
        }

        public bool ScrollViewportVerticallyByPage(ScrollDirection direction)
        {
            int lastVisibleLine = _wpfTextView.TextViewLines.LastVisibleLine.Start.GetContainingLine().LineNumber;
            int firstVisibleLine = _wpfTextView.TextViewLines.FirstVisibleLine.Start.GetContainingLine().LineNumber;
            int pageSize = lastVisibleLine - firstVisibleLine;

            _wpfTextView.TextViewLines.Scroll(direction, pageSize);
            return true;
        }

        public void ScrollViewportVerticallyByPixels(double distanceToScroll)
        {
            throw new NotImplementedException();
        }
    }
}
