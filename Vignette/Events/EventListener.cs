// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Vignette.Framework.Extensions;

namespace Vignette.Framework.Events;

public abstract class EventListener : ExtensionComponent
{
    public EventListener(Extension extension)
        : base(extension)
    {
    }

    public abstract void Invoke(object data);
}
