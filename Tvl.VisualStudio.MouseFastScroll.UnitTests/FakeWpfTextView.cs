// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests
{
    using System;
    using System.ComponentModel.Composition.Hosting;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Formatting;
    using Microsoft.VisualStudio.Text.Projection;
    using Microsoft.VisualStudio.Utilities;

    internal class FakeWpfTextView : IWpfTextView
    {
        private readonly IEditorOptions _editorOptions;

        public FakeWpfTextView(ExportProvider exportProvider)
        {
            _editorOptions = new FakeEditorOptions(exportProvider, this);
        }

        public FrameworkElement VisualElement => throw new NotImplementedException();

        public Brush Background
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public IWpfTextViewLineCollection TextViewLines => throw new NotImplementedException();

        public IFormattedLineSource FormattedLineSource => throw new NotImplementedException();

        public ILineTransformSource LineTransformSource => throw new NotImplementedException();

        public double ZoomLevel
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public bool InLayout => throw new NotImplementedException();

        public IViewScroller ViewScroller => throw new NotImplementedException();

        public ITextCaret Caret => throw new NotImplementedException();

        public ITextSelection Selection => throw new NotImplementedException();

        public ITrackingSpan ProvisionalTextHighlight
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public ITextViewRoleSet Roles => throw new NotImplementedException();

        public ITextBuffer TextBuffer => throw new NotImplementedException();

        public IBufferGraph BufferGraph => throw new NotImplementedException();

        public ITextSnapshot TextSnapshot => throw new NotImplementedException();

        public ITextSnapshot VisualSnapshot => throw new NotImplementedException();

        public ITextViewModel TextViewModel => throw new NotImplementedException();

        public ITextDataModel TextDataModel => throw new NotImplementedException();

        public double MaxTextRightCoordinate => throw new NotImplementedException();

        public double ViewportLeft
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public double ViewportTop => throw new NotImplementedException();

        public double ViewportRight => throw new NotImplementedException();

        public double ViewportBottom => throw new NotImplementedException();

        public double ViewportWidth => throw new NotImplementedException();

        public double ViewportHeight => throw new NotImplementedException();

        public double LineHeight => throw new NotImplementedException();

        public bool IsClosed => throw new NotImplementedException();

        public IEditorOptions Options => _editorOptions;

        public bool IsMouseOverViewOrAdornments => throw new NotImplementedException();

        public bool HasAggregateFocus => throw new NotImplementedException();

        public PropertyCollection Properties => throw new NotImplementedException();

        ITextViewLineCollection ITextView.TextViewLines => throw new NotImplementedException();

        public event EventHandler<BackgroundBrushChangedEventArgs> BackgroundBrushChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler<ZoomLevelChangedEventArgs> ZoomLevelChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler<TextViewLayoutChangedEventArgs> LayoutChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler ViewportLeftChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler ViewportHeightChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler ViewportWidthChanged
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler<MouseHoverEventArgs> MouseHover
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler Closed
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler LostAggregateFocus
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public event EventHandler GotAggregateFocus
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void DisplayTextLineContainingBufferPosition(SnapshotPoint bufferPosition, double verticalDistance, ViewRelativePosition relativeTo)
        {
            throw new NotImplementedException();
        }

        public void DisplayTextLineContainingBufferPosition(SnapshotPoint bufferPosition, double verticalDistance, ViewRelativePosition relativeTo, double? viewportWidthOverride, double? viewportHeightOverride)
        {
            throw new NotImplementedException();
        }

        public IAdornmentLayer GetAdornmentLayer(string name)
        {
            throw new NotImplementedException();
        }

        public ISpaceReservationManager GetSpaceReservationManager(string name)
        {
            throw new NotImplementedException();
        }

        public SnapshotSpan GetTextElementSpan(SnapshotPoint point)
        {
            throw new NotImplementedException();
        }

        public IWpfTextViewLine GetTextViewLineContainingBufferPosition(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public void QueueSpaceReservationStackRefresh()
        {
            throw new NotImplementedException();
        }

        ITextViewLine ITextView.GetTextViewLineContainingBufferPosition(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }
    }
}
