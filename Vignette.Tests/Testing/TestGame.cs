// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Evergine.Framework;

namespace Vignette.Framework.Tests.Testing;

public class TestGame : Game
{
    protected override Scene[] InitializeScenes()
        => new[] { new Scene() };
}
