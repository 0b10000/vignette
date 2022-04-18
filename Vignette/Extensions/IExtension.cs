// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

namespace Vignette.Framework.Extensions;

public interface IExtension : IExtensionMetadata
{
    string Name { get; }
    string Author { get; }
    string Description { get; }
}
