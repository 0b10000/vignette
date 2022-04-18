// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System.Collections.Generic;
using Evergine.Common.System;
using Vignette.Framework.Logging;

namespace Vignette.Framework.Vendor.Modules;

public class Console : Module
{
    public override ModuleType Type => ModuleType.Global;
    private readonly ILogger logger;
    private readonly Dictionary<string, Stopwatch> timers = new Dictionary<string, Stopwatch>();
    private readonly Dictionary<string, int> counters = new Dictionary<string, int>();

    public Console(VendorExtension extension)
        : base(extension)
    {
        logger = Logger.GetLogger($"Extension {Extension.Identifier}");
    }

    public void Log(object message)
            => logger.Information(message.ToString());

    public void Warn(object message)
        => logger.Warning(message.ToString());

    public void Debug(object message)
        => logger.Debug(message.ToString());

    public void Time(string label = "default")
    {
        if (timers.ContainsKey(label))
            return;

        var timer = new Stopwatch();
        timers.Add(label, timer);
        timer.Start();
    }

    public void TimeLog(string label = "default")
    {
        if (!timers.TryGetValue(label, out var timer))
        {
            Warn($@"Timer ""{label}"" doesn't exist.");
            return;
        }

        Log($"{label}: {timer.ElapsedMilliseconds}ms");
    }

    public void TimeEnd(string label = "default")
    {
        if (!timers.TryGetValue(label, out var timer))
        {
            Warn($@"Timer ""{label}"" doesn't exist.");
            return;
        }

        timer.Stop();
        timers.Remove(label);
        Log($"{label}: {timer.ElapsedMilliseconds}ms - timer ended");
    }

    public void Count(string label = "default")
    {
        if (!counters.TryGetValue(label, out int count))
        {
            count = 0;
            counters.Add(label, count);
        }

        count++;
        Log($"{label}: {count}");
    }

    public void CountReset(string label = "default")
    {
        if (!counters.ContainsKey(label))
            return;

        counters[label] = 0;
    }
}
