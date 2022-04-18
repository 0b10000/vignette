// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;

namespace Vignette.Framework.Extensions;

public struct ExtensionDependency : IExtensionMetadata
{
    public string Identifier { get; set; }
    public Version Version { get; set; }
    public bool Required { get; set; }

    public bool Equals(IExtensionMetadata other)
        => other.Identifier == Identifier && other.Version == Version;

    public override bool Equals(object obj)
        => obj is ExtensionDependency dependency && Equals(dependency);

    public override int GetHashCode()
        => HashCode.Combine(Identifier, Version, Required);

    public override string ToString()
        => $@"Dependency {Identifier}, Version={Version}, Required={Required}";

    public static bool operator ==(ExtensionDependency left, ExtensionDependency right)
        => left.Equals(right);

    public static bool operator !=(ExtensionDependency left, ExtensionDependency right)
        => !(left == right);
}
