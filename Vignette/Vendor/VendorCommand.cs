// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Vignette.Framework.Commands;
using Vignette.Framework.Util;

namespace Vignette.Framework.Vendor;

public class VendorCommand : Command
{
    public override string Name { get; }
    public override IReadOnlyList<Parameter> Parameters => parameters ?? base.Parameters;
    private readonly ScriptObject callback;
    private readonly IReadOnlyList<Parameter> parameters;

    public VendorCommand(string name, ScriptObject callback, IReadOnlyList<Parameter> parameters)
        : base(callback.Engine.GetExtension())
    {
        if ((!callback?.IsFunction() ?? false) && (!callback?.IsAsyncFunction() ?? false))
            throw new ArgumentException(@"Callback must be a function.", nameof(callback));

        Name = name;
        this.callback = callback;
        this.parameters = parameters;
    }

    protected override Task<object> Perform(object[] args, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        var value = callback.Invoke(false, args);

        if (callback.IsAsyncFunction())
            return value.ToTask();

        return Task.FromResult(value);
    }
}
