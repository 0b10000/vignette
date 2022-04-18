// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Concurrent;
using System.Threading;
using Vignette.Framework.Platform;

namespace Vignette.Framework.Tests.Testing;

/// <summary>
/// A host that runs in the background.
/// </summary>
public class HeadlessHost : Host
{
    private Thread updateThread;
    private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();
    private readonly CancellationTokenSource token = new CancellationTokenSource();

    public void Perform(Action action) => queue.Enqueue(action);

    protected override void Initialize(Game game)
    {
        base.Initialize(game);

        Load();

        updateThread = new Thread((obj) =>
        {
            while (!((CancellationToken)obj).IsCancellationRequested)
            {
                while (queue.TryDequeue(out var action))
                    action.Invoke();

                Update();
            }
        });

        updateThread.Start(token.Token);
    }

    protected override void Dispose(bool disposing)
    {
        token.Cancel();
        base.Dispose(disposing);
    }
}
