// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Vignette.Framework.Vendor;

namespace Vignette.Framework.Tests.Extensions;

public class TestVendorExtension : VendorExtension
{
    public string Code { get; set; }

    protected override Document GetDocument()
        => new Document(new DocumentInfo { Category = ModuleCategory.Standard }, Code);
}
