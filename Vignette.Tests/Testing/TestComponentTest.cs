// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using NUnit.Framework;

namespace Vignette.Tests.Testing;

public class TestComponentTest : TestComponent
{
    [Test]
    public void TestInsertComponent()
    {
        Assert.That(Managers.EntityManager.Count, Is.GreaterThanOrEqualTo(1));
    }
}
