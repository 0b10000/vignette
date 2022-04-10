// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Threading;
using NUnit.Framework;
using Vignette.Tests.Testing;

namespace Vignette.Tests.Platform;

public class HeadlessHostTest
{
    [Test]
    public void TestHeadlessHostLoading()
    {
        using var host = new HeadlessHost();
        host.Run(new TestGame());

        Thread.Sleep(100);

        Assert.That(host.IsLoaded, Is.True);
    }

    [Test]
    public void TestHeadlessHostPerforming()
    {
        using var host = new HeadlessHost();
        host.Run(new TestGame());

        bool isUpdated = false;
        host.Perform(() => isUpdated = true);

        Thread.Sleep(100);

        Assert.That(isUpdated, Is.True);
    }
}
