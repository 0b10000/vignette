// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NUnit.Framework;
using Vignette.Framework.IO.Serialization;

namespace Vignette.Framework.Tests.IO;

public class EnumFlagSerializationTests
{
    [Test]
    public void TestDeserializeEnumFlags()
    {
        var value = JsonSerializer.Deserialize<TestEnum>(@"[ ""One"", ""Three"" ]");
        Assert.That(value.HasFlag(TestEnum.One), Is.True);
        Assert.That(value.HasFlag(TestEnum.Two), Is.False);
        Assert.That(value.HasFlag(TestEnum.Three), Is.True);
    }

    [Test]
    public void TestDeserializeNoFlags()
    {
        var value = JsonSerializer.Deserialize<TestEnum>("[]");
        Assert.That(value, Is.EqualTo(TestEnum.None));
    }

    [Test]
    public void TestSerializationFlags()
    {
        var value = TestEnum.One | TestEnum.Two;
        string stringified = JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = false });
        Assert.That(stringified, Is.EqualTo(@"[""One"",""Two""]"));
    }

    [Test]
    public void TestSerializationNoFlags()
    {
        var value = TestEnum.None;
        string stringified = JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = false });
        Assert.That(stringified, Is.EqualTo("[]"));
    }

    [Flags]
    [JsonConverter(typeof(EnumFlagConverter<TestEnum>))]
    private enum TestEnum
    {
        [Annotations.Ignore]
        None = 0,
        One = 1,
        Two = 2,
        Three = 4,
    }
}
