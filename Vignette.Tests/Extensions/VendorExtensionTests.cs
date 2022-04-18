// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using NUnit.Framework;
using Vignette.Framework.Tests.Testing;
using Vignette.Framework.Vendor;

namespace Vignette.Framework.Tests.Extensions;

public class VendorExtensionTests : TestScene
{
    [Test]
    public void TestCreateCommand()
    {
        string code = @"
import vignette from 'vignette';

vignette.commands.register('hello', () => 'hello world');
";

        var ext = new TestVendorExtension { Code = code };
        ext.Load();

        Assert.That(ext.Components, Is.Not.Empty);
        Assert.That(ext.Components[0].Extension, Is.EqualTo(ext));
        Assert.That((ext.Components[0] as VendorCommand).Invoke(Array.Empty<object>()), Is.EqualTo("hello world"));
    }
}
