// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Threading;
using System.Threading.Tasks;
using Evergine.Framework;
using Evergine.Framework.Services;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Vignette.Framework.Tests.Testing;

public abstract class TestScene : Component
{
    private HeadlessHost host;
    private Task runTask;
    private Entity entity;

    [OneTimeSetUp]
    public void OneTimeSetUpForRunner()
    {
        if (!TestUtils.IsTestHost)
            return;

        host = new HeadlessHost();
        runTask = Task.Factory.StartNew(() => host.Run(new TestGame()), TaskCreationOptions.LongRunning);

        while (!host.IsLoaded)
        {
            checkForErrors();
            Thread.Sleep(10);
        }

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

            entity = new Entity("Test");
            entity.AddComponent(this);

            var sceneManager = host.Game.Container.Resolve<ScreenContextManager>();
            sceneManager.CurrentContext[0].Managers.EntityManager.Add(entity);

            checkForErrors();

            ready = true;
        });

        while (!ready)
            Thread.Sleep(10);
    }

    [TearDown]
    public void TearDownFromRunner()
    {
        checkForErrors();
    }

    [OneTimeTearDown]
    public void OneTimeTearDownFromRunner()
    {
        if (!TestUtils.IsTestHost)
            return;

        host.Perform(() =>
        {
            Managers.EntityManager.Remove(entity);
            checkForErrors();
        });

        try
        {
            // TODO: Investigate NullReferenceException caused by non-existent graphics backend.
            host.Dispose();
        }
        catch
        {
        }
    }

    private void checkForErrors()
    {
        if (runTask.Exception != null)
            throw runTask.Exception;
    }
}
