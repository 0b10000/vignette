// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vignette.Framework.Commands;
using Vignette.Framework.Tests.Extensions;

namespace Vignette.Framework.Tests.Commands;

public class TestCommand : Command
{
    public override string Name { get; }
    public override IReadOnlyList<Parameter> Parameters => parameters ?? base.Parameters;
    private readonly IReadOnlyList<Parameter> parameters;
    private readonly Func<object[], object> action;

    public TestCommand(Func<object[], object> action, IReadOnlyList<Parameter> parameters = null)
        : base(TestExtension.Default)
    {
        this.action = action;
        this.parameters = parameters;
    }

    protected override Task<object> Perform(object[] args, CancellationToken token = default)
        => Task.FromResult(action.Invoke(args));
}
