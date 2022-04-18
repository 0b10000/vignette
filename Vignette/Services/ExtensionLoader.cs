// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using Evergine.Framework.Services;
using Vignette.Framework.Extensions;

namespace Vignette.Framework.Services;

public class ExtensionLoader : Service
{
    public IReadOnlyCollection<Extension> Extensions => extensions;
    private readonly List<Extension> extensions = new List<Extension>();

    public void Load(Extension extension)
    {
        if (!IsActivated)
            throw new InvalidOperationException(@"Cannot load extensions while service is disabled.");

        if (extension == null)
            throw new ArgumentNullException(nameof(extension));

        if (Extensions.Contains(extension, EqualityComparer<IExtension>.Default))
            throw new ExtensionLoadException(@$"Extension ""{extension}"" is already loaded.");

        var missing = extension.Dependencies?
            .Where(dep => !Extensions.Any(ext => ext.Identifier == dep.Identifier && ext.Version >= dep.Version) && dep.Required);

        if (missing.Any())
            throw new ExtensionLoadException(@$"Failed to load ""{extension}"" as one or more dependencies have not been met.");

        extensions.Add(extension);
    }

    public void Unload(Extension extension)
    {
        if (!IsActivated)
            throw new InvalidOperationException(@"Cannot unload extensions while service is disabled.");

        if (extension == null)
            throw new ArgumentNullException(nameof(extension));

        if (!Extensions.Contains(extension, EqualityComparer<IExtension>.Default))
            throw new ExtensionLoadException(@$"Extension ""{extension}"" is not loaded.");

        var allDependencies = Extensions
            .SelectMany(ext => ext.Dependencies)
            .Distinct();

        if (allDependencies.Any(dep => extension.Identifier == dep.Identifier && extension.Version >= dep.Version && dep.Required))
            throw new ExtensionLoadException(@$"Failed to unload ""{extension}"" as one or more extensions depend on it.");

        extensions.Remove(extension);
    }

    public void LoadAll(IEnumerable<Extension> loadables)
        => extensions.AddRange(loadables);

    public void UnloadAll()
        => extensions.Clear();
}

public class ExtensionLoadException : Exception
{
    public ExtensionLoadException(string message)
        : base(message)
    {
    }
}
