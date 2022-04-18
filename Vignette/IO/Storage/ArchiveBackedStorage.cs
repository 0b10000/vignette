// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using DotNet.Globbing;

namespace Vignette.Framework.IO.Storage;

/// <summary>
/// Storage backed by an archive.
/// </summary>
public class ArchiveBackedStorage : IStorage, IDisposable
{
    private readonly ZipArchive archive;
    private readonly IEnumerable<string> entries;
    private bool isDisposed;

    public ArchiveBackedStorage(Stream archiveStream)
    {
        archive = new ZipArchive(archiveStream, ZipArchiveMode.Read);
        entries = archive.Entries.Select(e => e.FullName);
    }

    public ArchiveBackedStorage(string path)
        : this(File.OpenRead(path))
    {
    }

    public bool CreateDirectory(string path)
    {
        throw new NotSupportedException(@"Cannot create directories in an archive.");
    }

    public void Delete(string path)
    {
        throw new UnauthorizedAccessException(@"Archives are read-only.");
    }

    public void DeleteDirectory(string path)
    {
        throw new NotSupportedException(@"Cannot delete directories in an archive.");
    }

    public IEnumerable<string> EnumerateDirectories(string path, string pattern = "*")
    {
        throw new NotSupportedException(@"Cannot enumerate directories in an archive.");
    }

    public IEnumerable<string> EnumerateFiles(string path, string pattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly)
    {
        var glob = Glob.Parse(pattern);
        return entries.Where(e => glob.IsMatch(e));
    }

    public bool Exists(string path)
    {
        return entries.Contains(path);
    }

    public bool ExistsDirectory(string path)
    {
        throw new NotSupportedException(@"Cannot enumerate directories in an archive.");
    }

    public Stream Open(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
    {
        if (!Exists(path))
            throw new FileNotFoundException(null, path);

        if (mode != FileMode.Open || access != FileAccess.Read)
            throw new UnauthorizedAccessException(@"Archives are read-only");

        lock (archive)
        {
            var entry = archive.GetEntry(path);
            var readable = entry.Open();
            var writable = new MemoryStream();

            readable.CopyTo(writable);
            writable.Position = 0;

            return writable;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed)
            return;

        archive.Dispose();

        isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
