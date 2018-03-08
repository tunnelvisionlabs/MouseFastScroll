// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(IMouseProcessorProvider))]
    [Order]
    [ContentType("text")]
    [Name("FastScroll")]
    [TextViewRole(PredefinedTextViewRoles.Interactive)]
    internal class FastScrollProvider : IMouseProcessorProvider
    {
        public IMouseProcessor GetAssociatedProcessor(IWpfTextView wpfTextView)
        {
            if (wpfTextView == null)
            {
                return null;
            }

            wpfTextView.Options.SetOptionValue(DefaultWpfViewOptions.EnableMouseWheelZoomId, false);
            return new FastScrollProcessor(wpfTextView);
        }
    }
}
