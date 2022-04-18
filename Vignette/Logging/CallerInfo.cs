// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Runtime.CompilerServices;

namespace Vignette.Framework.Logging;

public struct CallerInfo : IEquatable<CallerInfo>
{
    public readonly string Message;
    public readonly string FilePath;
    public readonly string MemberName;
    public readonly int LineNumber;

    private CallerInfo(string message, string filePath, string memberName, int line)
    {
        Message = message;
        FilePath = filePath;
        MemberName = memberName;
        LineNumber = line;
    }

    public static CallerInfo Trace(string message, [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int line = 0)
        => new CallerInfo(message, filePath, memberName, line);

    public bool Equals(CallerInfo other)
        => other.Message.Equals(Message) && other.FilePath.Equals(FilePath)
            && other.MemberName.Equals(MemberName) && other.LineNumber.Equals(LineNumber);

    public override bool Equals(object obj)
        => obj is CallerInfo info && Equals(info);

    public override int GetHashCode()
        => HashCode.Combine(Message, FilePath, MemberName, LineNumber);

    public override string ToString()
        => $"{FilePath}:{MemberName}:{LineNumber}";

    public static bool operator ==(CallerInfo left, CallerInfo right)
        => left.Equals(right);

    public static bool operator !=(CallerInfo left, CallerInfo right)
        => !(left == right);
}
