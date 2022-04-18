// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using NUnit.Framework;
using Vignette.Framework.IO.Storage;

namespace Vignette.Framework.Tests.IO;

public class VirtualStorageTests
{
    [Test]
    public void TestMounting()
    {
        var vfs = new VirtualStorage();
        var a = new InMemoryStorage();
        var b = new InMemoryStorage();
        var root = new InMemoryStorage();

        vfs.Mount("/", root);
        vfs.Mount("/a", a);
        vfs.Mount("/a/b", b);

        Assert.That(vfs.GetStorage("/a/"), Is.Not.Null);
        Assert.That(vfs.GetStorage("/a/b/"), Is.Not.Null);
        Assert.That(vfs.GetStorage("/"), Is.Not.Null);
    }
}
