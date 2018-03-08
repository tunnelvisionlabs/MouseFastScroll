// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests
{
    using System;
    using Microsoft.VisualStudio.Text;

    internal class FakeTextSnapshotLine : ITextSnapshotLine
    {
        private readonly int _lineNumber;
        private readonly SnapshotSpan _extentIncludingLineBreak;

        public FakeTextSnapshotLine(int lineNumber, SnapshotSpan extentIncludingLineBreak)
        {
            _lineNumber = lineNumber;
            _extentIncludingLineBreak = extentIncludingLineBreak;
        }

        public ITextSnapshot Snapshot => _extentIncludingLineBreak.Snapshot;

        public SnapshotSpan Extent => throw new NotImplementedException();

        public SnapshotSpan ExtentIncludingLineBreak => _extentIncludingLineBreak;

        public int LineNumber => _lineNumber;

        public SnapshotPoint Start => _extentIncludingLineBreak.Start;

        public int Length => throw new NotImplementedException();

        public int LengthIncludingLineBreak => throw new NotImplementedException();

        public SnapshotPoint End => throw new NotImplementedException();

        public SnapshotPoint EndIncludingLineBreak => _extentIncludingLineBreak.End;

        public int LineBreakLength => throw new NotImplementedException();

        public string GetLineBreakText()
        {
            throw new NotImplementedException();
        }

        public string GetText()
        {
            return Extent.GetText();
        }

        public string GetTextIncludingLineBreak()
        {
            return ExtentIncludingLineBreak.GetText();
        }
    }
}