// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using NUnit.Framework;
using Vignette.Framework.Commands;

namespace Vignette.Framework.Tests.Commands;

public class CommandTests
{
    [Test]
    public void TestCommandDispatch()
    {
        var cmd = new TestCommand(_ => true);
        Assert.That(() => cmd.Invoke<bool>(Array.Empty<object>()), Is.True);
    }

    [Test]
    public void TestCommandDispatchWithParameters()
    {
        var cmd = new TestCommand(args => args[0]);
        Assert.That(() => cmd.Invoke<int>(new object[] { 42 }), Is.EqualTo(42));
    }

    [TestCase(typeof(StringParameter), "Hello World", @"""Goodbye World""", "Goodbye World")]
    [TestCase(typeof(NumberParameter), 69, "420", 420)]
    [TestCase(typeof(BooleanParameter), false, "True", true)]
    public void TestCommandDispatchWithStringifiedParameters(Type paramType, object defaultValue, string stringValue, object parsedValue)
    {
        var param = (Parameter)Activator.CreateInstance(paramType);
        param.Name = "Param1";
        param.Default = defaultValue;

        if (param is NumberParameter numParam)
        {
            numParam.Minimum = 0;
            numParam.Maximum = 1000;
        }

        var cmd = new TestCommand(args => args[0], new[] { param });

        Assert.That(() => cmd.Invoke(Array.Empty<object>()), Is.EqualTo(defaultValue));
        Assert.That(() => cmd.Invoke($"-Param1 {stringValue}"), Is.EqualTo(parsedValue));
    }
}
