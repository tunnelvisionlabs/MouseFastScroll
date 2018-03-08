// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests
{
    using System;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;

    internal class FakeViewScroller : IViewScroller
    {
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
            throw new NotImplementedException();
        }

        public void ScrollViewportVerticallyByLines(ScrollDirection direction, int count)
        {
            throw new NotImplementedException();
        }

        public bool ScrollViewportVerticallyByPage(ScrollDirection direction)
        {
            throw new NotImplementedException();
        }

        public void ScrollViewportVerticallyByPixels(double distanceToScroll)
        {
            throw new NotImplementedException();
        }
    }
}
