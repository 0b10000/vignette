// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.IO;
using Evergine.Framework.Services;
using Vignette.Framework.IO.Storage;

namespace Vignette.Framework.Services;

public class Storage : Service, IVirtualStorage
{
    private readonly VirtualStorage storage = new VirtualStorage();

    public Storage()
    {
        storage.Mount("/", new NativeStorage(AppDomain.CurrentDomain.BaseDirectory));
        storage.Mount("/data/", new NativeStorage(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
    }

    public bool CreateDirectory(string path)
        => storage.CreateDirectory(path);

    public void Delete(string path)
        => storage.Delete(path);

    public void DeleteDirectory(string path)
        => storage.DeleteDirectory(path);

    public IEnumerable<string> EnumerateDirectories(string path, string pattern = "*")
        => storage.EnumerateDirectories(path, pattern);

    public IEnumerable<string> EnumerateFiles(string path, string pattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        => storage.EnumerateFiles(path, pattern, searchOptions);

    public bool Exists(string path)
        => storage.Exists(path);

    public bool ExistsDirectory(string path)
        => storage.ExistsDirectory(path);

    public IStorage GetStorage(string basePath)
        => storage.GetStorage(basePath);

    public void Mount(string basePath, IStorage storage)
        => this.storage.Mount(basePath, storage);

    public Stream Open(string path, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite)
        => storage.Open(path, mode, access);

    public void Unmount(string basePath)
        => storage.Unmount(basePath);
}
