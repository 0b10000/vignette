// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using Evergine.Framework;
using NUnit.Framework;
using Vignette.Framework.Extensions;
using Vignette.Framework.Services;
using Vignette.Framework.Tests.Extensions;
using Vignette.Framework.Tests.Testing;

namespace Vignette.Framework.Tests.Services;

public class ExtensionLoaderTests : TestScene
{
    private ExtensionLoader service;

    [SetUp]
    public void SetUp()
    {
        service = Application.Current.Container.Resolve<ExtensionLoader>();
    }

    [TearDown]
    public void TearDown()
    {
        service.UnloadAll();
        service = null;
    }

    [Test]
    public void TestLoading()
    {
        var a = new TestExtension("a", new Version("1.0.0"));
        var b = new TestExtension("b", dependencies: new[] { new ExtensionDependency { Identifier = "a", Version = new Version("0.0.1"), Required = true } });

        Assert.That(() => service.Load(a), Throws.Nothing);
        Assert.That(() => service.Load(b), Throws.Nothing);
        Assert.That(service.Extensions.Count, Is.EqualTo(2));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestDependencyRequire(bool isRequired)
    {
        var a = new TestExtension("a", dependencies: new[] { new ExtensionDependency { Identifier = "b", Version = new Version("1.0.0"), Required = isRequired } });

        Assert.That(() => service.Load(a), isRequired ? Throws.InstanceOf<ExtensionLoadException>() : Throws.Nothing);
        Assert.That(service.Extensions.Count, Is.EqualTo(isRequired ? 0 : 1));
    }

    [Test]
    public void TestDependencyOlder()
    {
        var a = new TestExtension("a", new Version("1.0.0"));
        var b = new TestExtension("b", dependencies: new[] { new ExtensionDependency { Identifier = "a", Version = new Version("2.0.0"), Required = true } });

        Assert.That(() => service.Load(a), Throws.Nothing);
        Assert.That(() => service.Load(b), Throws.InstanceOf<ExtensionLoadException>());
        Assert.That(service.Extensions.Count, Is.EqualTo(1));
    }
}
