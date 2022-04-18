// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.IO;

namespace Vignette.Framework.Logging;

public class LogListenerTextWriter : LogListener
{
    private readonly TextWriter writer;

    public LogListenerTextWriter(Stream stream)
    {
        writer = new StreamWriter(stream);
    }

    public LogListenerTextWriter(TextWriter writer)
    {
        this.writer = writer;
    }

    protected override void OnNewMessage(string message)
    {
        lock (writer)
            writer.WriteLine(message);
    }

    protected override void Flush()
    {
        writer.Flush();
    }
}
