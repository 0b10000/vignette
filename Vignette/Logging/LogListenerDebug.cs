// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Diagnostics;

namespace Vignette.Framework.Logging;

public class LogListenerDebug : LogListener
{
    protected override void OnNewMessage(string message)
    {
        Debug.WriteLine(message);
    }

    protected override void Flush()
    {
        Debug.Flush();
    }
}
