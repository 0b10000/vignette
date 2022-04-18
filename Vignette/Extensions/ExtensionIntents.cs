// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Text.Json.Serialization;
using Vignette.Framework.Annotations;
using Vignette.Framework.IO.Serialization;

namespace Vignette.Framework.Extensions;

/// <summary>
/// Defines capabilities of an extension.
/// </summary>
[Flags]
[JsonConverter(typeof(EnumFlagConverter<ExtensionIntents>))]
public enum ExtensionIntents
{
    /// <summary>
    /// The extension does not have any intents.
    /// </summary>
    [Ignore]
    None = 0,

    /// <summary>
    /// The extension intends to access the user's file system.
    /// </summary>
    Storage = 1,
}
