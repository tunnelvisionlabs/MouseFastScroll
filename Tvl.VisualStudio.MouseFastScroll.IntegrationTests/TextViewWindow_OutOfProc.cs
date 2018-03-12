// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    public abstract class TextViewWindow_OutOfProc : OutOfProcComponent
    {
        protected TextViewWindow_OutOfProc(VisualStudioInstance visualStudioInstance)
            : base(visualStudioInstance)
        {
            TextViewWindowInProc = CreateInProcComponent(visualStudioInstance);
        }

        internal TextViewWindow_InProc TextViewWindowInProc
        {
            get;
        }

        internal abstract TextViewWindow_InProc CreateInProcComponent(VisualStudioInstance visualStudioInstance);

        public int GetCaretPosition()
            => TextViewWindowInProc.GetCaretPosition();
    }
}
