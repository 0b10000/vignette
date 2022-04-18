// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Vignette.Framework.Annotations;

namespace Vignette.Framework.Extensions;

/// <summary>
/// The base class for constituents adding features.
/// </summary>
public abstract class ExtensionComponent
{
    /// <summary>
    /// Determines whether the component has been loaded.
    /// </summary>
    [Ignore]
    public bool IsLoaded { get; private set; }

    /// <summary>
    /// Determines whether the component has been destroyed.
    /// </summary>
    [Ignore]
    public bool IsDestroyed { get; private set; }

    /// <summary>
    /// The owning extension.
    /// </summary>
    [Ignore]
    public Extension Extension { get; private set; }

    public ExtensionComponent(Extension extension)
    {
        Extension = extension ?? throw new System.ArgumentNullException(nameof(extension));
        Extension.AddComponent(this);
    }

    /// <summary>
    /// Loads this component.
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
    /// Destroys this component.
    /// </summary>
    [Ignore]
    public void Destroy()
    {
        if (!IsLoaded || IsDestroyed)
            return;

        IsDestroyed = true;
        IsLoaded = false;

        OnDestroy();

        Extension.RemoveComponent(this);
        Extension = null;
    }

    /// <summary>
    /// Called once during loading.
    /// </summary>
    protected virtual void OnLoad()
    {
    }

    /// <summary>
    /// Called once during destruction.
    /// </summary>
    protected virtual void OnDestroy()
    {
    }
}
