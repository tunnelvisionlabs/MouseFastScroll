// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests
{
    using Microsoft.VisualStudio.Text.Editor;
    using Tvl.VisualStudio.MouseFastScroll.UnitTests.Fakes;
    using Xunit;

    public class FastScrollProviderTests
    {
        [Fact]
        public void NullInput()
        {
            var provider = CompositionHelper.GetProvider(out _);
            Assert.Null(provider.GetAssociatedProcessor(null));
        }

        [Fact]
        public void AttachesToView()
        {
            var provider = CompositionHelper.GetProvider(out var exportProvider);
            var wpfTextView = new FakeWpfTextView(exportProvider, new FakeTextSnapshot(string.Empty));

            var processor = provider.GetAssociatedProcessor(wpfTextView);

            Assert.NotNull(processor);
        }

        [Fact]
        public void DisablesZoom()
        {
            var provider = CompositionHelper.GetProvider(out var exportProvider);
            var wpfTextView = new FakeWpfTextView(exportProvider, new FakeTextSnapshot(string.Empty));

            Assert.True(wpfTextView.Options.GetOptionValue(DefaultWpfViewOptions.EnableMouseWheelZoomId));

            var processor = provider.GetAssociatedProcessor(wpfTextView);
            Assert.False(wpfTextView.Options.GetOptionValue(DefaultWpfViewOptions.EnableMouseWheelZoomId));
        }
    }
}
