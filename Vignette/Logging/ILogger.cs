// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;

namespace Vignette.Framework.Logging;

public interface ILogger
{
    void Debug(string message);
    void Verbose(string message);
    void Information(string message);
    void Warning(string message);
    void Error(string message, Exception exception = null);
}
