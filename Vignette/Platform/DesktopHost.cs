// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using Evergine.Common.Audio;
using Evergine.Common.Graphics;
using Evergine.OpenAL;
using Evergine.OpenGL;
using Evergine.Framework.Graphics;
using Evergine.Framework.Services;

namespace Vignette.Platform;

/// <summary>
/// A host capable of launching and displaying contents to a window.
/// </summary>
public abstract class DesktopHost : Host
{
    protected virtual string Name => @"Vignette";
    protected Window Window { get; private set; }

    private WindowsSystem windowsSystem;
    private GraphicsContext graphicsContext;
    private const int default_width = 1280;
    private const int default_height = 720;

    protected virtual AudioDevice CreateAudioDevice() => new ALAudioDevice();
    protected virtual GraphicsContext CreateGraphicsContext() => new GLGraphicsContext();
    protected abstract WindowsSystem CreateWindowsSystem();

    protected override void Initialize(Game game)
    {
        base.Initialize(game);

        Game.Container.RegisterInstance(windowsSystem = CreateWindowsSystem());
        Game.Container.RegisterInstance(graphicsContext = CreateGraphicsContext());
        Game.Container.RegisterInstance(CreateAudioDevice());

        Window = windowsSystem.CreateWindow(Name, default_width, default_height);

        Prepare();

        graphicsContext.CreateDevice();

        var presenter = Game.Container.Resolve<GraphicsPresenter>();
        var swapChain = graphicsContext.CreateSwapChain(new SwapChainDescription
        {
            SurfaceInfo = Window.SurfaceInfo,
            Width = Window.Width,
            Height = Window.Height,
            ColorTargetFormat = PixelFormat.R8G8B8A8_UNorm,
            ColorTargetFlags = TextureFlags.RenderTarget | TextureFlags.ShaderResource,
            DepthStencilTargetFormat = PixelFormat.D24_UNorm_S8_UInt,
            DepthStencilTargetFlags = TextureFlags.DepthStencil,
            SampleCount = TextureSampleCount.None,
            IsWindowed = true,
            RefreshRate = 60
        });

        presenter.AddDisplay("Default", new Display(Window, swapChain));

        windowsSystem.Run(Load, Update);
    }

    protected virtual void Prepare()
    {
    }

    protected override void Update(TimeSpan time)
    {
        base.Update(time);
        Game?.DrawFrame(time);
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);
        Window?.Dispose();
        windowsSystem?.Dispose();
    }
}
