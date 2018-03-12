// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.UnitTests
{
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using Microsoft.VisualStudio.Text.Editor;
    using Tvl.VisualStudio.MouseFastScroll.UnitTests.Fakes;
    using Xunit;

    internal static class CompositionHelper
    {
        public static IMouseProcessorProvider GetProvider(out ExportProvider exportProvider)
        {
            var catalog = new AggregateCatalog(
                new AssemblyCatalog(typeof(FastScrollProvider).Assembly),
                new AssemblyCatalog(typeof(DefaultWpfViewOptions).Assembly));
            exportProvider = new CompositionContainer(catalog);

            var providers = exportProvider.GetExportedValues<IMouseProcessorProvider>();
            var provider = providers.OfType<FastScrollProvider>().FirstOrDefault();
            Assert.NotNull(provider);

            return provider;
        }

        public static IMouseProcessor GetProcessor(string content, out ExportProvider exportProvider, out FakeWpfTextView wpfTextView)
        {
            var provider = GetProvider(out exportProvider);
            wpfTextView = new FakeWpfTextView(exportProvider, new FakeTextSnapshot(content));
            var processor = provider.GetAssociatedProcessor(wpfTextView);
            Assert.NotNull(processor);

            return processor;
        }
    }
}
