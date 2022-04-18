// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;

namespace Vignette.Framework.Logging;

public sealed class Logger : ILogger
{
    public string Name { get; }
    public bool Enabled { get; set; } = true;
    public bool BroadcastGlobal { get; set; } = true;
    public event Action<LogMessage> MessageLogged;
    public static readonly ILogger Default = new Logger { BroadcastGlobal = false };
    public static event Action<LogMessage> MessageLoggedGlobal;
    private static readonly Dictionary<string, Logger> loggers = new Dictionary<string, Logger>();

    private Logger(string name = null)
    {
        Name = name;
    }

    public void Debug(string message)
        => broadcast(new LogMessage(message, Name, LogLevel.Debug));

    public void Verbose(string message)
        => broadcast(new LogMessage(message, Name, LogLevel.Verbose));

    public void Information(string message)
        => broadcast(new LogMessage(message, Name, LogLevel.Information));

    public void Warning(string message)
        => broadcast(new LogMessage(message, Name, LogLevel.Warning));

    public void Error(string message, Exception exception = null)
        => broadcast(new LogMessage(message, Name, LogLevel.Error, exception));

    private void broadcast(LogMessage message)
    {
        if (!Enabled)
            return;

        MessageLogged?.Invoke(message);

        if (BroadcastGlobal)
            MessageLoggedGlobal?.Invoke(message);
    }

    public static Logger GetLogger(string name)
    {
        if (!loggers.TryGetValue(name, out var logger))
        {
            logger = new Logger(name);
            loggers.Add(name, logger);
        }

        return logger;
    }
}
