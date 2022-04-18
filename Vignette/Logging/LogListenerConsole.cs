// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;

namespace Vignette.Framework.Logging;

public class LogListenerConsole : LogListenerTextWriter
{
    public LogListenerConsole()
        : base(Console.Out)
    {
    }
}
