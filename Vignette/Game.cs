// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Evergine.Framework;
using Evergine.Framework.Services;
using Evergine.Platform;

namespace Vignette;

public partial class Game : Application
{
    public Game()
    {
        Container.RegisterType<Settings>();
        Container.RegisterType<Clock>();
        Container.RegisterType<TimerFactory>();
        Container.RegisterType<Random>();
        Container.RegisterType<ErrorHandler>();
        Container.RegisterType<ScreenContextManager>();
        Container.RegisterType<ForegroundTaskSchedulerService>();
        Container.RegisterType<WorkActionScheduler>();
        Container.RegisterType<GraphicsPresenter>();
        Container.RegisterType<AssetsDirectory>();
        Container.RegisterType<AssetsService>();
    }

    public override void Initialize()
    {
        base.Initialize();

        var screenContextManager = Container.Resolve<ScreenContextManager>();
        var screenContext = new ScreenContext(InitializeScenes());
        screenContextManager.To(screenContext);
    }

    protected virtual Scene[] InitializeScenes()
    {
        var assetsService = Container.Resolve<AssetsService>();
        return new[] { assetsService.Load<Scene>(EvergineContent.Scenes.MyScene_wescene) };
    }
}
