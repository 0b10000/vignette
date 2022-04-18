// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Vignette.Framework.Annotations;

namespace Vignette.Framework.Vendor;

public abstract class Module
{
    protected readonly VendorExtension Extension;

    [Ignore]
    public virtual ModuleType Type => ModuleType.Document;

    [Ignore]
    public virtual string MemberName => GetType().Name;

    protected Module(VendorExtension extension)
    {
        Extension = extension;
    }

    public enum ModuleType
    {
        Document,
        Global,
    }
}
