// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using Microsoft.ClearScript;
using Vignette.Framework.Vendor;

namespace Vignette.Framework.Util;

public static class ScriptEngineExtensions
{
    /// <summary>
    /// Gets the owning vendor extension for this engine.
    /// </summary>
    /// <param name="engine">The script engine.</param>
    /// <returns>The vendor extension owning this engine.</returns>
    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    public static VendorExtension GetExtension(this ScriptEngine engine)
    {
        if (engine is null)
            throw new ArgumentNullException(nameof(engine));

        if (engine.Global.GetProperty(ModuleBootstrapper.EXTENSION_GLOBAL_KEY) is not VendorExtension extension)
            throw new InvalidOperationException($"This {nameof(ScriptEngine)} is not owned by a {nameof(VendorExtension)}.");

        return extension;
    }
}
