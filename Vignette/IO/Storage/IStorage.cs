// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Collections.Generic;
using System.IO;

namespace Vignette.Framework.IO.Storage;

public interface IStorage
{
    Stream Open(string path, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite);
    bool Exists(string path);
    void Delete(string path);
    IEnumerable<string> EnumerateFiles(string path, string pattern = "*", SearchOption searchOptions = SearchOption.TopDirectoryOnly);
    bool ExistsDirectory(string path);
    bool CreateDirectory(string path);
    void DeleteDirectory(string path);
    IEnumerable<string> EnumerateDirectories(string path, string pattern = "*");
}
