// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using Microsoft.ClearScript;
using Vignette.Framework.Events;
using Vignette.Framework.Util;

namespace Vignette.Framework.Vendor;

public class VendorEventListener : EventListener, IEquatable<ScriptObject>
{
    private readonly ScriptObject callback;

    public VendorEventListener(ScriptObject callback)
        : base(callback.Engine.GetExtension())
    {
        if (!callback?.IsFunction() ?? false)
            throw new ArgumentException("Callback must be a function.", nameof(callback));

        this.callback = callback;
    }

    public override void Invoke(object data)
        => callback.Invoke(false, data);

    public bool Equals(ScriptObject other)
        => callback.Equals(other);
}
