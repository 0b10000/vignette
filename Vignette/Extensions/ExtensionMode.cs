// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

namespace Vignette.Framework.Extensions;

/// <summary>
/// Hints how an extension should run.
/// </summary>
public enum ExtensionMode
{
    /// <summary>
    /// Suggests that this extension is running for release.
    /// </summary>
    Production,

    /// <summary>
    /// Suggests that this extension is running for development enabling debugging features.
    /// </summary>
    Development,
}
