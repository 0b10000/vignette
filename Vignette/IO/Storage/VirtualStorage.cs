// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vignette.Framework.IO.Storage;

/// <summary>
/// Storage with no backing allowing mounting points for other <see cref="IStorage"/>s.
/// </summary>
public class VirtualStorage : IVirtualStorage
{
    private readonly Dictionary<string, IStorage> storages = new Dictionary<string, IStorage>();

    public VirtualStorage(IStorage root = null)
    {
        if (root != null)
            Mount("/", root);
    }

    public void Mount(string basePath, IStorage storage)
    {
        if (string.IsNullOrEmpty(basePath))
            throw new ArgumentException("Path cannot be null or empty", nameof(basePath));

        if (storages.ContainsKey(basePath))
            throw new InvalidOperationException();

        basePath = basePath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        if (!basePath.StartsWith(Path.AltDirectorySeparatorChar))
            basePath = Path.AltDirectorySeparatorChar + basePath;

        if (!basePath.EndsWith(Path.AltDirectorySeparatorChar))
            basePath += Path.AltDirectorySeparatorChar;

        storages.Add(basePath, storage);
    }

    public void Unmount(string basePath)
    {
        storages.Remove(basePath);
    }

    public IStorage GetStorage(string path)
    {
        var result = resolve(path);
        return result.IStorage;
    }

    public bool CreateDirectory(string path)
    {
        var (storage, relativePath) = resolve(path);
        return storage?.CreateDirectory(relativePath) ?? false;
    }

    public void Delete(string path)
    {
        var (storage, relativePath) = resolve(path);
        storage?.Delete(relativePath);
    }

    public void DeleteDirectory(string path)
    {
        var (storage, relativePath) = resolve(path);
        storage?.DeleteDirectory(relativePath);
    }

    public IEnumerable<string> EnumerateDirectories(string path, string pattern = "*")
    {
        var (storage, relativePath) = resolve(path);
        return storage?.EnumerateDirectories(relativePath, pattern);
    }

    public IEnumerable<string> EnumerateFiles(string path, string pattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
    {
        var (storage, relativePath) = resolve(path);
        return storage?.EnumerateFiles(relativePath, pattern, searchOptions);
    }

    public bool Exists(string path)
    {
        var (storage, relativePath) = resolve(path);
        return storage?.Exists(relativePath) ?? false;
    }

    public bool ExistsDirectory(string path)
    {
        var (storage, relativePath) = resolve(path);
        return storage?.ExistsDirectory(relativePath) ?? false;
    }

    public Stream Open(string path, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite)
    {
        var (storage, relativePath) = resolve(path);
        return storage?.Open(relativePath, mode, access);
    }

    private ResolveResult resolve(string path)
    {
        path = resolvePath(path);

        for (int i = path.Length; i >= 0; i--)
        {
            if (path[i - 1] != Path.AltDirectorySeparatorChar)
                continue;

            if (!storages.TryGetValue(path[..i], out var storage))
                continue;

            string relativePath = i == path.Length ? string.Empty : path[i..];

            return new ResolveResult(storage, relativePath);
        }

        return default;
    }

    private static readonly string relative_to_parent = "..";
    private static readonly string relative_to_current = ".";

    private static string resolvePath(string path)
    {
        path = path.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        string[] components = path.Split(Path.AltDirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

        for (int i = components.Length - 1; i >= 0; i--)
        {
            string component = components[i];

            if (component == relative_to_parent)
            {
                components[i] = null;

                if (i > 0)
                    components[i - 1] = null;
            }

            if (component == relative_to_current)
            {
                components[i] = null;
            }
        }

        return Path.AltDirectorySeparatorChar + string.Join(Path.AltDirectorySeparatorChar, components.Where(c => !string.IsNullOrEmpty(c)));
    }

    private record struct ResolveResult(IStorage IStorage, string Path);
}
