// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.IO;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Vignette.Framework.IO.Storage;
using Vignette.Framework.Extensions;

namespace Vignette.Framework.Vendor;

public class FolderBasedVendorExtension : StorageBasedVendorExtension
{
    public new NativeStorage Storage => (NativeStorage)base.Storage;
    public override ExtensionMode Mode => ExtensionMode.Development;
    private static readonly string source_map = "//# sourceMappingURL=";

    public FolderBasedVendorExtension(string path)
        : base(path)
    {
    }

    protected override IStorage CreateStorage(string path)
        => new NativeStorage(path);

    protected override Document GetDocument()
    {
        using var stream = Storage.Open("extension.js", FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);

        Uri sourceMapUri = null;
        string code = reader.ReadToEnd();

        if (Storage.Exists("extension.js.map"))
            sourceMapUri = new Uri(Storage.GetFullPath("extension.js.map"));

        if (Storage.Exists("extension.js.map.min"))
            sourceMapUri = new Uri(Storage.GetFullPath("extension.js.map.min"));

        if (code.Contains("//# sourceMappingURL="))
            sourceMapUri = new Uri(code[(code.IndexOf(source_map) + source_map.Length)..]);

        var info = new DocumentInfo(new Uri(Storage.GetFullPath("extension.js")))
        {
            Category = ModuleCategory.Standard,
            SourceMapUri = sourceMapUri,
        };

        return new Document(info, code);
    }
}
