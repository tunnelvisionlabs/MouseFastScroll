// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    public abstract class OutOfProcComponent
    {
        protected OutOfProcComponent(VisualStudioInstance visualStudioInstance)
        {
            VisualStudioInstance = visualStudioInstance;
        }

        protected VisualStudioInstance VisualStudioInstance
        {
            get;
        }

        internal static TInProcComponent CreateInProcComponent<TInProcComponent>(VisualStudioInstance visualStudioInstance)
            where TInProcComponent : InProcComponent
        {
            return visualStudioInstance.ExecuteInHostProcess<TInProcComponent>(type: typeof(TInProcComponent), methodName: "Create");
        }
    }
}
