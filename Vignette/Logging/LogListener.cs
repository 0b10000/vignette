// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;

namespace Vignette.Framework.Logging;

public abstract class LogListener : IDisposable
{
    public bool IsDisposed { get; private set; }
    public int MessagesLogged { get; private set; }
    public LogLevel Level { get; set; } = LogLevel.Information;

    private int logEveryCount = 1;
    public int LogEveryCount
    {
        get => logEveryCount;
        set => logEveryCount = Math.Max(value, 1);
    }

    protected abstract void OnNewMessage(string message);

    protected virtual void Flush()
    {
    }

    protected virtual string GetTextFormatted(LogMessage message)
    {
        return $"[{message.Timestamp:dd/MM/yyyy hh:mm:ss tt}] [{message.Level}]{(string.IsNullOrEmpty(message.Channel) ? string.Empty : $" [{message.Channel}]")} {message.Message}{(message.Exception != null ? $"\n{message.Exception}" : string.Empty)}";
    }

    private void handleNewMessage(LogMessage message)
    {
        if (message.Level < Level)
            return;

        OnNewMessage(GetTextFormatted(message));
        MessagesLogged++;

        if ((MessagesLogged % LogEveryCount) == 0)
            Flush();
    }

    protected virtual void Dispose(bool isDisposing)
    {
        IsDisposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public static implicit operator Action<LogMessage>(LogListener listener)
    {
        return listener.handleNewMessage;
    }
}
