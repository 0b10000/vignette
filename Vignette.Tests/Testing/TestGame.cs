// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Evergine.Framework;

namespace Vignette.Tests.Testing;

public class TestGame : Game
{
    public Scene CurrentScene { get; private set; }

    protected override Scene[] InitializeScenes()
        => new[] { CurrentScene = new Scene() };
}
