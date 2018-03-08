// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Utilities;

    internal class FakeTextSnapshot : ITextSnapshot
    {
        private readonly string _content;
        private readonly ReadOnlyCollection<ITextSnapshotLine> _lines;

        public FakeTextSnapshot(string content)
        {
            _content = content;

            var lines = new List<ITextSnapshotLine>();
            for (int i = content.IndexOf('\n'); i >= 0; i = content.IndexOf('\n', i + 1))
            {
                int start = lines.Count == 0 ? 0 : lines.Last().EndIncludingLineBreak;
                int endIncludingLineBreak = i + 1;
                lines.Add(new FakeTextSnapshotLine(lines.Count, new SnapshotSpan(this, start, endIncludingLineBreak)));
            }

            int lastLineStart = lines.Count == 0 ? 0 : lines.Last().EndIncludingLineBreak;
            lines.Add(new FakeTextSnapshotLine(lines.Count, new SnapshotSpan(this, lastLineStart, content.Length)));

            _lines = lines.AsReadOnly();
        }

        public ITextBuffer TextBuffer => throw new NotImplementedException();

        public IContentType ContentType => throw new NotImplementedException();

        public ITextVersion Version => throw new NotImplementedException();

        public int Length => _content.Length;

        public int LineCount => throw new NotImplementedException();

        public IEnumerable<ITextSnapshotLine> Lines => _lines;

        public char this[int position] => throw new NotImplementedException();

        public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            throw new NotImplementedException();
        }

        public ITrackingPoint CreateTrackingPoint(int position, PointTrackingMode trackingMode)
        {
            throw new NotImplementedException();
        }

        public ITrackingPoint CreateTrackingPoint(int position, PointTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
        {
            throw new NotImplementedException();
        }

        public ITrackingSpan CreateTrackingSpan(Span span, SpanTrackingMode trackingMode)
        {
            throw new NotImplementedException();
        }

        public ITrackingSpan CreateTrackingSpan(Span span, SpanTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
        {
            throw new NotImplementedException();
        }

        public ITrackingSpan CreateTrackingSpan(int start, int length, SpanTrackingMode trackingMode)
        {
            throw new NotImplementedException();
        }

        public ITrackingSpan CreateTrackingSpan(int start, int length, SpanTrackingMode trackingMode, TrackingFidelityMode trackingFidelity)
        {
            throw new NotImplementedException();
        }

        public ITextSnapshotLine GetLineFromLineNumber(int lineNumber)
        {
            throw new NotImplementedException();
        }

        public ITextSnapshotLine GetLineFromPosition(int position)
        {
            foreach (var line in Lines.Reverse())
            {
                if (line.Start == position)
                {
                    return line;
                }

                if (line.ExtentIncludingLineBreak.Contains(position))
                {
                    return line;
                }
            }

            throw new NotImplementedException($"position {position} not found");
        }

        public int GetLineNumberFromPosition(int position)
        {
            throw new NotImplementedException();
        }

        public string GetText(Span span)
        {
            throw new NotImplementedException();
        }

        public string GetText(int startIndex, int length)
        {
            throw new NotImplementedException();
        }

        public string GetText()
        {
            throw new NotImplementedException();
        }

        public char[] ToCharArray(int startIndex, int length)
        {
            throw new NotImplementedException();
        }

        public void Write(TextWriter writer, Span span)
        {
            throw new NotImplementedException();
        }

        public void Write(TextWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
