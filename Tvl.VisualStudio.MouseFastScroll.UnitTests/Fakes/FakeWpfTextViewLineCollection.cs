// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests.Fakes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Formatting;
    using Xunit;

    internal class FakeWpfTextViewLineCollection : IWpfTextViewLineCollection
    {
        private readonly ITextSnapshot _snapshot;
        private readonly ReadOnlyCollection<IWpfTextViewLine> _lines;

        private int _viewableLines = 20;
        private int _firstVisibleLine = 0;

        public FakeWpfTextViewLineCollection(ITextSnapshot snapshot)
        {
            _snapshot = snapshot;
            _lines = _snapshot.Lines.Select<ITextSnapshotLine, IWpfTextViewLine>(line => new FakeWpfTextViewLine(line)).ToList().AsReadOnly();
        }

        public ReadOnlyCollection<IWpfTextViewLine> WpfTextViewLines => _lines;

        public IWpfTextViewLine FirstVisibleLine => this[_firstVisibleLine];

        public IWpfTextViewLine LastVisibleLine
        {
            get
            {
                int lastVisibleLine = Math.Min(_lines.Count - 1, _firstVisibleLine + _viewableLines - 1);
                return this[lastVisibleLine];
            }
        }

        public SnapshotSpan FormattedSpan => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        ITextViewLine ITextViewLineCollection.FirstVisibleLine => FirstVisibleLine;

        ITextViewLine ITextViewLineCollection.LastVisibleLine => LastVisibleLine;

        public IWpfTextViewLine this[int index] => _lines[index];

        ITextViewLine IList<ITextViewLine>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        internal void Scroll(ScrollDirection direction, int lines)
        {
            Assert.True(lines >= 0, "Assertion failed: lines >= 0");

            if (direction == ScrollDirection.Down)
            {
                lines = Math.Min(lines, _lines.Count - _firstVisibleLine - 1);
                if (lines > 0)
                {
                    _firstVisibleLine += lines;
                }
            }
            else
            {
                Assert.Equal(ScrollDirection.Up, direction);
                int newTopLine = Math.Max(0, _firstVisibleLine - lines);
                _firstVisibleLine = newTopLine;
            }
        }

        public void Add(ITextViewLine item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ITextViewLine item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsBufferPosition(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ITextViewLine[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public TextBounds GetCharacterBounds(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ITextViewLine> GetEnumerator()
        {
            return WpfTextViewLines.GetEnumerator();
        }

        public int GetIndexOfTextLine(ITextViewLine textLine)
        {
            throw new NotImplementedException();
        }

        public Geometry GetLineMarkerGeometry(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        public Geometry GetLineMarkerGeometry(SnapshotSpan bufferSpan, bool clipToViewport, Thickness padding)
        {
            throw new NotImplementedException();
        }

        public Geometry GetMarkerGeometry(SnapshotSpan bufferSpan, bool clipToViewport, Thickness padding)
        {
            throw new NotImplementedException();
        }

        public Geometry GetMarkerGeometry(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        public Collection<TextBounds> GetNormalizedTextBounds(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        public SnapshotSpan GetTextElementSpan(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public Geometry GetTextMarkerGeometry(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        public Geometry GetTextMarkerGeometry(SnapshotSpan bufferSpan, bool clipToViewport, Thickness padding)
        {
            throw new NotImplementedException();
        }

        public IWpfTextViewLine GetTextViewLineContainingBufferPosition(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }

        public ITextViewLine GetTextViewLineContainingYCoordinate(double y)
        {
            throw new NotImplementedException();
        }

        public Collection<ITextViewLine> GetTextViewLinesIntersectingSpan(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(ITextViewLine item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, ITextViewLine item)
        {
            throw new NotImplementedException();
        }

        public bool IntersectsBufferSpan(SnapshotSpan bufferSpan)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ITextViewLine item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ITextViewLine ITextViewLineCollection.GetTextViewLineContainingBufferPosition(SnapshotPoint bufferPosition)
        {
            return GetTextViewLineContainingBufferPosition(bufferPosition);
        }
    }
}
