// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Linq;
using Evergine.Framework;
using Vignette.Framework.Extensions;
using Vignette.Framework.Services;

namespace Vignette.Framework.Vendor.Modules;

public class Extensions : Module
{
    public string Name => Extension.Name;
    public ExtensionMode Mode => Extension.Mode;
    public string Author => Extension.Author;
    public string Version => Extension.Version.ToString(3);
    public string Description => Extension.Description;
    private readonly ExtensionLoader extensionService;

    public Extensions(VendorExtension extension)
        : base(extension)
    {
        extensionService = Application.Current.Container.Resolve<ExtensionLoader>();
    }

    public bool Has(string id) => extensionService.Extensions.Any(e => e.Identifier == id);
}
