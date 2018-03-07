namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTestService
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    [Guid("78D5A8B5-1634-434B-802D-E3E4A46B1AA6")]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideMenuResource("Menus.ctmenu", version: 1)]
    public sealed class IntegrationTestServicePackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();
            IntegrationTestServiceCommands.Initialize(this);
        }
    }
}
