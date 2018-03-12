// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests
{
    using System.Collections.Immutable;
    using Xunit;

    [CollectionDefinition(nameof(SharedIntegrationHostFixture))]
    public sealed class SharedIntegrationHostFixture : ICollectionFixture<VisualStudioInstanceFactory>
    {
        public const string MouseFastScrollPackageId = "Tvl.VisualStudio.MouseFastScroll.7DFA0DD1-8052-464D-9A1A-5EADC10A84B0";
        public const string IntegrationTestPackageId = "Tvl.VisualStudio.MouseFastScroll.IntegrationTestService";

        public static readonly ImmutableHashSet<string> RequiredPackageIds = ImmutableHashSet.Create<string>();
    }
}
