// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;

namespace Vignette.Framework.Extensions;

public interface IExtensionMetadata : IEquatable<IExtensionMetadata>
{
    string Identifier { get; }
    Version Version { get; }
}
