// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

namespace Vignette.Framework.IO.Storage;

public interface IVirtualStorage : IStorage
{
    void Mount(string basePath, IStorage storage);
    void Unmount(string basePath);
    IStorage GetStorage(string basePath);
}
