// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;

namespace Vignette.Framework.Commands;

public abstract class Parameter : IEquatable<Parameter>
{
    public string Name { get; set; }
    public object Default { get; set; }
    public bool Required { get; set; }
    internal int Order { get; set; }

    internal virtual object Parse(string arg)
        => Default;

    public bool Equals(Parameter other)
        => other.Name.Equals(Name) && other.Default.Equals(Default);

    public override bool Equals(object obj)
        => Equals(obj as Parameter);

    public override int GetHashCode()
        => HashCode.Combine(Name, Default);
}

public class StringParameter : Parameter
{
    internal override string Parse(string arg)
    {
        if (!string.IsNullOrEmpty(arg))
            return arg[1..^1];

        return string.Empty;
    }
}

public class NumberParameter : Parameter
{
    public double Minimum { get; set; }
    public double Maximum { get; set; }

    internal override object Parse(string arg)
    {
        if (double.TryParse(arg, out double value))
            return Math.Min(Math.Max(value, Minimum), Maximum);

        return base.Parse(arg);
    }
}

public class BooleanParameter : Parameter
{
    internal override object Parse(string arg)
    {
        if (bool.TryParse(arg, out bool value))
            return value;

        return base.Parse(arg);
    }
}
