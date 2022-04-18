// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Evergine.Framework;
using Vignette.Framework.IO.Storage;

namespace Vignette.Framework.Vendor;

public abstract class StorageBasedVendorExtension : VendorExtension
{
    protected IStorage Storage { get; private set; }
    private IVirtualStorage storageService;
    private readonly string path;

    protected StorageBasedVendorExtension(string path)
    {
        this.path = path;
    }

    protected abstract IStorage CreateStorage(string path);

    protected override void OnLoad()
    {
        base.OnLoad();

        var storage = new VirtualStorage();
        storage.Mount("/", CreateStorage(path));

        if (Intents.HasFlag(Extensions.ExtensionIntents.Storage))
            storage.Mount("/data/", new NativeStorage($"./data/extensions/{Identifier}"));

        storageService = Application.Current?.Container.Resolve<IVirtualStorage>();
        storageService?.Mount($"/extensions/{Identifier}/", Storage);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        storageService?.Unmount($"/extensions/{Identifier}/");
        storageService = null;
    }
}
