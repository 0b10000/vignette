// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Threading;
using System.Threading.Tasks;
using Evergine.Framework;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Vignette.Tests.Testing;

public abstract class TestComponent : Component
{
    private HeadlessHost host;
    private Task runTask;
    private Entity entity;

    [OneTimeSetUp]
    public void OneTimeSetUpForRunner()
    {
        host = new HeadlessHost();
        runTask = Task.Factory.StartNew(() => host.Run(new TestGame()), TaskCreationOptions.LongRunning);

        while (!host.IsLoaded)
        {
            checkForErrors();
            Thread.Sleep(10);
        }
    }

    [SetUp]
    public void SetUpForRunner()
    {
        if (!TestUtils.IsTestHost)
            return;

        bool ready = false;

        var context = TestExecutionContext.CurrentContext;

        host.Perform(() =>
        {
            TestExecutionContext.CurrentContext.CurrentResult = context.CurrentResult;
            TestExecutionContext.CurrentContext.CurrentTest = context.CurrentTest;
            TestExecutionContext.CurrentContext.CurrentCulture = context.CurrentCulture;
            TestExecutionContext.CurrentContext.CurrentPrincipal = context.CurrentPrincipal;
            TestExecutionContext.CurrentContext.CurrentRepeatCount = context.CurrentRepeatCount;
            TestExecutionContext.CurrentContext.CurrentUICulture = context.CurrentUICulture;

            entity = new Entity("Test")
                .AddComponent(this);

            var game = host.Game as TestGame;
            game.CurrentScene.Managers.EntityManager.Add(entity);
            checkForErrors();

            ready = true;
        });

        while (!ready)
            Thread.Sleep(10);
    }

    [TearDown]
    public void TearDownFromRunner()
    {
        host.Perform(() =>
        {
            var game = host.Game as TestGame;
            game.CurrentScene.Managers.EntityManager.Remove(entity);
            checkForErrors();
        });
    }

    [OneTimeTearDown]
    public void OneTimeTearDownFromRunner()
    {
        host.Dispose();
    }

    private void checkForErrors()
    {
        if (runTask.Exception != null)
            throw runTask.Exception;
    }
}
