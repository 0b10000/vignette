// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using Vignette.Framework.Annotations;

namespace Vignette.Framework.Extensions;

public abstract class Extension : IExtension
{
    public virtual string Identifier { get; }
    public virtual Version Version { get; }
    public virtual string Name { get; }
    public virtual string Author { get; }
    public virtual string Description { get; }
    public virtual ExtensionIntents Intents { get; }
    public virtual ExtensionMode Mode => ExtensionMode.Production;
    public virtual IReadOnlyList<ExtensionDependency> Dependencies { get; }

    [Ignore]
    public bool IsLoaded { get; private set; }

    [Ignore]
    public IReadOnlyList<ExtensionComponent> Components => components;

    private readonly List<ExtensionComponent> components = new List<ExtensionComponent>();

    /// <summary>
    /// Loads this extension
    /// </summary>
    [Ignore]
    public void Load()
    {
        if (IsLoaded)
            return;

        OnLoad();

        IsLoaded = true;
    }

    /// <summary>
    /// Destroys this extension.
    /// </summary>
    [Ignore]
    public void Destroy()
    {
        if (!IsLoaded)
            return;

        IsLoaded = false;

        OnDestroy();

        for (int i = components.Count - 1; i > 0; i--)
            RemoveComponent(components[i]);
    }

    /// <summary>
    /// Adds this component.
    /// </summary>
    [Ignore]
    public void AddComponent(ExtensionComponent component)
    {
        if (component is null)
            throw new ArgumentNullException(nameof(component));

        if (components.Contains(component))
            return;

        components.Add(component);
        component.Load();
    }

    /// <summary>
    /// Removes this component.
    /// </summary>
    [Ignore]
    public void RemoveComponent(ExtensionComponent component)
    {
        if (component is null)
            throw new ArgumentNullException(nameof(component));

        if (!components.Contains(component))
            return;

        component.Destroy();
        components.Remove(component);
    }

    protected virtual void OnLoad()
    {
    }

    protected virtual void OnDestroy()
    {
    }

    public bool Equals(IExtension other)
        => Name.Equals(other.Name) && Author.Equals(other.Author)
            && Description.Equals(other.Description) && Identifier.Equals(other.Identifier)
            && Version.Equals(other.Version);

    public bool Equals(IExtensionMetadata other)
        => Identifier.Equals(other.Identifier) && Version.Equals(other.Version);

    public override string ToString() => $@"{GetType().Name} {Identifier}, Version={Version.ToString(3)}";
}
