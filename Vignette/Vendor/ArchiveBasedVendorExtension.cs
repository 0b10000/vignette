// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.IO;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Vignette.Framework.IO.Storage;

namespace Vignette.Framework.Vendor;

public class ArchiveBasedVendorExtension : StorageBasedVendorExtension
{
    public ArchiveBasedVendorExtension(string archivePath)
        : base(archivePath)
    {
    }

    protected override IStorage CreateStorage(string path)
        => new ArchiveBackedStorage(path);

    protected override Document GetDocument()
    {
        using var stream = Storage.Open("extension.js", FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        return new Document(new DocumentInfo { Category = ModuleCategory.Standard }, reader.ReadToEnd());
    }
}
