// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ClearScript;
using Vignette.Framework.Annotations;
using Vignette.Framework.Vendor;

namespace Vignette.Framework.Events;

public class EventEmitter
{
    private readonly List<Listener> listeners = new List<Listener>();

    public void AddListener(string name, EventListener listener)
        => listeners.Add(new Listener(name, listener));

    public void RemoveListener(string name, EventListener listener)
        => listeners.Remove(listeners.FirstOrDefault(e => e.EventName == name && e.EventListener == listener));

    public void Emit(string name, object data)
    {
        foreach (var listener in GetListenersfor(name))
            listener.Invoke(data);
    }

    [Ignore]
    public IEnumerable<EventListener> GetListenersfor(string name)
        => listeners.Where(e => e.EventName == name).Select(e => e.EventListener);

    private record struct Listener(string EventName, EventListener EventListener);
}

public static class EventListenerExtensions
{
    public static void AddListener(this EventEmitter emitter, string name, ScriptObject callback)
        => emitter.AddListener(name, new VendorEventListener(callback));

    public static void RemoveListener(this EventEmitter emitter, string name, ScriptObject callback)
    {
        var listener = emitter.GetListenersfor(name).FirstOrDefault(e => e is VendorEventListener vendor && vendor.Equals(callback));

        if (listener == null)
            throw new InvalidOperationException();

        emitter.RemoveListener(name, listener);
    }
}
