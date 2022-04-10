// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Reflection;

namespace Vignette.Tests.Testing;

public class TestUtils
{
    public static bool IsTestHost => isTesting.Value;
    private static readonly Lazy<bool> isTesting = new Lazy<bool>(() => Assembly.GetEntryAssembly().Location.Contains("testhost"));
}
