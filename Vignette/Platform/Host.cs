// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Diagnostics;

namespace Vignette.Framework.Platform;

/// <summary>
/// Base class for application hosts.
/// </summary>
public abstract class Host : IDisposable
{
    public Game Game { get; private set; }
    public bool IsLoaded { get; set; }
    public bool IsRunning { get; private set; }
    protected bool IsDisposed { get; private set; }
    private readonly Stopwatch clock = new Stopwatch();

    /// <summary>
    /// Initialies necessary components and runs this host.
    /// </summary>
    public void Run(Game game)
    {
        if (IsRunning)
            return;

        Initialize(game);

        IsRunning = true;
    }

    protected virtual void Initialize(Game game)
    {
        Game = game;
        Game.Container.RegisterInstance(this);
    }

    protected void Load()
    {
        if (IsLoaded)
            return;

        Game?.Initialize();
        OnLoad();

        IsLoaded = true;
    }

    protected void Update()
    {
        var elapsed = clock.Elapsed;
        clock.Restart();
        OnUpdate(elapsed);
    }

    protected virtual void OnLoad()
    {
    }

    protected virtual void OnUpdate(TimeSpan time)
    {
        Game?.UpdateFrame(time);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (IsDisposed)
            return;

        Game?.Dispose();

        IsDisposed = true;
    }

    /// <summary>
    /// Disposes resources used by this host.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
