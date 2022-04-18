// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using NUnit.Framework;

namespace Vignette.Framework.Tests.Testing;

public class TestSceneTest : TestScene
{
    [Test]
    public void TestInsertComponent()
    {
        Assert.That(Managers.EntityManager.Count, Is.GreaterThanOrEqualTo(1));
    }
}
