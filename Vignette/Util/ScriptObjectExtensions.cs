// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using Microsoft.ClearScript;

namespace Vignette.Framework.Util;

public static class ScriptObjectExtensions
{
    private static readonly string function = "Function";
    private static readonly string asnyc_function = "AsyncFunction";

    /// <summary>
    /// Gets the class name for this script object. Equivalent to getting the constructor's name.
    /// </summary>
    public static string GetClassName(this ScriptObject scriptObj)
    {
        if (scriptObj is null)
            throw new ArgumentNullException(nameof(scriptObj));

        return ((dynamic)scriptObj).constructor.name;
    }

    /// <summary>
    /// Gets whether this script object is a function or not.
    /// </summary>
    public static bool IsFunction(this ScriptObject scriptObj)
    {
        if (scriptObj is null)
            throw new ArgumentNullException(nameof(scriptObj));

        string className = GetClassName(scriptObj);
        return className == function;
    }

    /// <summary>
    /// Gets whether this script object is an async function or not.
    /// </summary>
    public static bool IsAsyncFunction(this ScriptObject scriptObj)
    {
        if (scriptObj is null)
            throw new ArgumentNullException(nameof(scriptObj));

        string className = GetClassName(scriptObj);
        return className == asnyc_function;
    }
}
