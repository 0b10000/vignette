// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using Vignette.Framework.Extensions;

namespace Vignette.Framework.Tests.Extensions;

public class TestExtension : Extension
{
    public static readonly Extension Default = new TestExtension("Test");
    public override string Identifier { get; }
    public override Version Version { get; }
    public override IReadOnlyList<ExtensionDependency> Dependencies { get; }

    public TestExtension(string identifier, Version version = null, IReadOnlyList<ExtensionDependency> dependencies = null)
    {
        Identifier = identifier;
        Version = version ?? new Version("0.0.0");
        Dependencies = dependencies ?? Array.Empty<ExtensionDependency>();
    }
}
