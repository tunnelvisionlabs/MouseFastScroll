// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests
{
    using System;
    using System.Windows.Input;
    using Xunit;

    public class FastScrollProcessorTests
    {
        [StaFact]
        public void ScrollWithoutControlPressed()
        {
            var processor = CompositionHelper.GetProcessor(out var exportProvider, out var wpfTextView);
            var args = new MouseWheelEventArgs(Mouse.PrimaryDevice, Environment.TickCount, 120);

            Assert.Equal(ModifierKeys.None, Keyboard.Modifiers & ModifierKeys.Control);
            Assert.False(args.Handled);
            var firstVisibleLine = wpfTextView.TextViewLines.FirstVisibleLine.Start.GetContainingLine().LineNumber;
            processor.PreprocessMouseWheel(args);
            Assert.False(args.Handled);
            Assert.Equal(firstVisibleLine, wpfTextView.TextViewLines.FirstVisibleLine.Start.GetContainingLine().LineNumber);
        }
    }
}
