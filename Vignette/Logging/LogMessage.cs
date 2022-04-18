// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;

namespace Vignette.Framework.Logging;

public struct LogMessage : IEquatable<LogMessage>
{
    public readonly DateTime Timestamp;
    public readonly LogLevel Level;
    public readonly string Channel;
    public readonly string Message;
    public readonly CallerInfo CallerInfo;
    public readonly Exception Exception;

    public LogMessage(string message, string channel = null, LogLevel level = LogLevel.Verbose, Exception exception = null, CallerInfo callerInfo = default)
    {
        Level = level;
        Channel = channel;
        Message = message;
        Timestamp = DateTime.Now;
        Exception = exception;
        CallerInfo = callerInfo;
    }

    public bool Equals(LogMessage other)
        => other.Level.Equals(Level) && other.Channel.Equals(Channel) && other.Message.Equals(Message)
            && other.CallerInfo.Equals(CallerInfo) && other.Exception.Equals(Exception);

    public override bool Equals(object obj)
        => obj is LogMessage message && Equals(message);

    public override int GetHashCode()
        => HashCode.Combine(Level, Channel, Message, CallerInfo, Exception);

    public static bool operator ==(LogMessage left, LogMessage right)
        => left.Equals(right);

    public static bool operator !=(LogMessage left, LogMessage right)
        => !(left == right);
}
