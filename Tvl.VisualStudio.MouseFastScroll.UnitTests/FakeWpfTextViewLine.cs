// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Media.TextFormatting;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Formatting;

    internal class FakeWpfTextViewLine : IWpfTextViewLine
    {
        private readonly ITextSnapshotLine _line;

        public FakeWpfTextViewLine(ITextSnapshotLine line)
        {
            _line = line;
        }

        public Rect VisibleArea => throw new NotImplementedException();

        public ReadOnlyCollection<TextLine> TextLines => throw new NotImplementedException();

        public object IdentityTag => throw new NotImplementedException();

        public ITextSnapshot Snapshot => _line.Snapshot;

        public bool IsFirstTextViewLineForSnapshotLine => throw new NotImplementedException();

        public bool IsLastTextViewLineForSnapshotLine => throw new NotImplementedException();

        public double Baseline => throw new NotImplementedException();

        public SnapshotSpan Extent => _line.Extent;

        public IMappingSpan ExtentAsMappingSpan => throw new NotImplementedException();

        public SnapshotSpan ExtentIncludingLineBreak => _line.ExtentIncludingLineBreak;

        public IMappingSpan ExtentIncludingLineBreakAsMappingSpan => throw new NotImplementedException();

        public SnapshotPoint Start => _line.Start;

        public int Length => _line.Length;

        public int LengthIncludingLineBreak => _line.LengthIncludingLineBreak;

        public SnapshotPoint End => _line.End;

        public SnapshotPoint EndIncludingLineBreak => _line.EndIncludingLineBreak;

        public int LineBreakLength => _line.LineBreakLength;

        public double Left => throw new NotImplementedException();

        public double Top => throw new NotImplementedException();

        public double Height => throw new NotImplementedException();

        public double TextTop => throw new NotImplementedException();

        public double TextBottom => throw new NotImplementedException();

        public double TextHeight => throw new NotImplementedException();

        public double TextLeft => throw new NotImplementedException();

        public double TextRight => throw new NotImplementedException();

        public double TextWidth => throw new NotImplementedException();

        public double Width => throw new NotImplementedException();

        public double Bottom => throw new NotImplementedException();

        public double Right => throw new NotImplementedException();

        public double EndOfLineWidth => throw new NotImplementedException();

        public double VirtualSpaceWidth => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public LineTransform LineTransform => throw new NotImplementedException();

        public LineTransform DefaultLineTransform => throw new NotImplementedException();

        public VisibilityState VisibilityState => throw new NotImplementedException();

        public double DeltaY => throw new NotImplementedException();

        public TextViewLineChange Change => throw new NotImplementedException();

        public bool ContainsBufferPosition(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public Microsoft.VisualStudio.Text.Formatting.TextBounds? GetAdornmentBounds(object identityTag)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<object> GetAdornmentTags(object providerTag)
        {
            throw new NotImplementedException();
        }

        public SnapshotPoint? GetBufferPositionFromXCoordinate(double xCoordinate, bool textOnly)
        {
            throw new NotImplementedException();
        }

        public SnapshotPoint? GetBufferPositionFromXCoordinate(double xCoordinate)
        {
            throw new NotImplementedException();
        }

        public Microsoft.VisualStudio.Text.Formatting.TextBounds GetCharacterBounds(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public Microsoft.VisualStudio.Text.Formatting.TextBounds GetCharacterBounds(VirtualSnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public TextRunProperties GetCharacterFormatting(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public Microsoft.VisualStudio.Text.Formatting.TextBounds GetExtendedCharacterBounds(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public Microsoft.VisualStudio.Text.Formatting.TextBounds GetExtendedCharacterBounds(VirtualSnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public VirtualSnapshotPoint GetInsertionBufferPositionFromXCoordinate(double xCoordinate)
        {
            throw new NotImplementedException();
        }

        public Collection<Microsoft.VisualStudio.Text.Formatting.TextBounds> GetNormalizedTextBounds(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        public SnapshotSpan GetTextElementSpan(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public VirtualSnapshotPoint GetVirtualBufferPositionFromXCoordinate(double xCoordinate)
        {
            throw new NotImplementedException();
        }

        public bool IntersectsBufferSpan(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }
    }
}
