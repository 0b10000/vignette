// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Collections;
using System.Linq;
using Evergine.Framework;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Vignette.Framework.Services;

namespace Vignette.Framework.Vendor.Modules;

public class Commands : Module
{
    private readonly CommandDispatcher commandsService;

    public Commands(VendorExtension extension)
        : base(extension)
    {
        commandsService = Application.Current.Container.Resolve<CommandDispatcher>();
    }

    public void Register(string name, ScriptObject callback)
        => Extension.AddComponent(new VendorCommand(name, callback, null));

    public void Unregister(string name)
        => Extension.RemoveComponent(Extension.Components.OfType<VendorCommand>().FirstOrDefault(cmd => cmd.Name == name));

    public object Invoke(string input, IList args)
        => commandsService.Invoke(input, args.Cast<object>().ToArray());

    public object InvokeAsync(string input, IList args)
        => commandsService.InvokeAsync(input, args.Cast<object>().ToArray()).ToPromise();
}
