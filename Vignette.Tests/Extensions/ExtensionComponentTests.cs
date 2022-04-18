// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Linq;
using NUnit.Framework;

namespace Vignette.Framework.Tests.Extensions;

public class ExtensionComponentTests
{
    [Test]
    public void TestComponentAddingWithExtension()
    {
        var ext = new TestExtension("a");
        var com = new TestExtensionComponent(ext);

        ext.AddComponent(com);
        Assert.That(com.IsLoaded, Is.True);
        Assert.That(ext.Components.Contains(com), Is.True);

        ext.RemoveComponent(com);
        Assert.That(com.IsDestroyed, Is.True);
        Assert.That(com.IsLoaded, Is.False);
        Assert.That(ext.Components.Contains(com), Is.False);
    }

    [Test]
    public void TestComponentAddingWithSelf()
    {
        var ext = new TestExtension("a");
        var com = new TestExtensionComponent(ext);

        Assert.That(com.IsLoaded, Is.True);
        Assert.That(ext.Components.Contains(com), Is.True);

        com.Destroy();

        Assert.That(com.IsDestroyed, Is.True);
        Assert.That(com.IsLoaded, Is.False);
        Assert.That(ext.Components.Contains(com), Is.False);
    }
}
