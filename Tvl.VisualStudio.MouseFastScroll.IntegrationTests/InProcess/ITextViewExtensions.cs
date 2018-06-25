// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.InProcess
{
    using System;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;

    internal static class ITextViewExtensions
    {
        public static SnapshotPoint? GetCaretPoint(this ITextView textView, Predicate<ITextSnapshot> match)
        {
            var caret = textView.Caret.Position;
            var span = textView.BufferGraph.MapUpOrDownToFirstMatch(new SnapshotSpan(caret.BufferPosition, 0), match);
            if (span.HasValue)
            {
                return span.Value.Start;
            }
            else
            {
                return null;
            }
        }

        public static ITextBuffer GetBufferContainingCaret(this ITextView textView, string contentType = "text")
        {
            var point = GetCaretPoint(textView, s => s.ContentType.IsOfType(contentType));
            return point.HasValue ? point.Value.Snapshot.TextBuffer : null;
        }
    }
}
