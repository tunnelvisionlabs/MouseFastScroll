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

    internal class FakeWpfTextViewLineCollection : IWpfTextViewLineCollection
    {
        private readonly ITextSnapshot _snapshot;
        private readonly IWpfTextViewLine[] _lines;

        private int _firstVisibleLine = 0;

        public FakeWpfTextViewLineCollection(ITextSnapshot snapshot)
        {
            _snapshot = snapshot;
            _lines = _snapshot.Lines.Select(line => new FakeWpfTextViewLine(line)).ToArray();
        }

        public ReadOnlyCollection<IWpfTextViewLine> WpfTextViewLines => throw new NotImplementedException();

        public IWpfTextViewLine FirstVisibleLine => this[_firstVisibleLine];

        public IWpfTextViewLine LastVisibleLine => throw new NotImplementedException();

        public SnapshotSpan FormattedSpan => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        ITextViewLine ITextViewLineCollection.FirstVisibleLine => FirstVisibleLine;

        ITextViewLine ITextViewLineCollection.LastVisibleLine => LastVisibleLine;

        public IWpfTextViewLine this[int index] => _lines[index];

        ITextViewLine IList<ITextViewLine>.this[int index]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        ITextViewLine ITextViewLineCollection.GetTextViewLineContainingBufferPosition(SnapshotPoint bufferPosition)
        {
            throw new NotImplementedException();
        }
    }
}
