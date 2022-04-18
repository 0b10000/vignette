// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Collections.Generic;
using System.IO;

namespace Vignette.Framework.IO.Storage;

/// <summary>
/// Storage backed by the platform's operating system.
/// </summary>
public class NativeStorage : IStorage
{
    public readonly string LocalBasePath;

    public NativeStorage(string localBasePath)
    {
        LocalBasePath = localBasePath;
    }

    public bool CreateDirectory(string path)
    {
        try
        {
            Directory.CreateDirectory(GetFullPath(path));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Delete(string path)
        => File.Delete(GetFullPath(path));

    public void DeleteDirectory(string path)
        => Directory.Delete(GetFullPath(path));

    public IEnumerable<string> EnumerateDirectories(string path, string pattern = "*")
        => Directory.EnumerateDirectories(GetFullPath(path), pattern);

    public IEnumerable<string> EnumerateFiles(string path, string pattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
        => Directory.EnumerateFiles(GetFullPath(path), pattern, searchOption);

    public bool Exists(string path)
        => File.Exists(GetFullPath(path));

    public bool ExistsDirectory(string path)
        => Directory.Exists(GetFullPath(path));

    public Stream Open(string path, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite)
        => File.Open(GetFullPath(path), mode, access);

    public string GetFullPath(string path)
        => Path.Combine(LocalBasePath, path).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
}
