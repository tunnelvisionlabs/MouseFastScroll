// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    internal class Editor_InProc : TextViewWindow_InProc
    {
        private Editor_InProc()
        {
        }

        public static Editor_InProc Create()
            => new Editor_InProc();

        public void Activate()
            => GetDTE().ActiveDocument.Activate();
    }
}
