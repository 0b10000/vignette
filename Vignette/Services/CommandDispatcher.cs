// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Evergine.Framework;
using Evergine.Framework.Services;
using Vignette.Framework.Commands;

namespace Vignette.Framework.Services;

public class CommandDispatcher : Service
{
    public IEnumerable<Command> Commands => extensionLoader.Extensions.OfType<Command>();

    [BindService]
    private ExtensionLoader extensionLoader;

    public Task<object> InvokeAsync(string input, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));

        string[] commandString = input.Split(' ', 2, StringSplitOptions.TrimEntries);
        return getCommand(commandString[0]).InvokeAsync(commandString[1], token);
    }

    public Task<object> InvokeAsync(string input, object[] args, CancellationToken token = default)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentNullException(nameof(input));

        return getCommand(input).InvokeAsync(args, token);
    }

    public object Invoke(string input)
        => InvokeAsync(input).GetAwaiter().GetResult();

    public object Invoke(string input, object[] args)
        => InvokeAsync(input, args).GetAwaiter().GetResult();

    private Command getCommand(string identifier)
    {
        var match = Command.IDENTIFIER_REGEX.Match(identifier);

        if (match.Length == 0)
            throw new ArgumentException("Invalid identifier.", nameof(identifier));

        string ns = match.Groups["Namespace"].Value;
        string id = match.Groups["Name"].Value;

        var extension = Commands.FirstOrDefault(cmd => cmd.Namespace == ns);

        if (extension == null)
            throw new KeyNotFoundException(@$"The namespace ""{ns}"" is not found.");

        var command = Commands.FirstOrDefault(cmd => cmd.Name == id);

        if (command == null)
            throw new KeyNotFoundException(@$"The command with the name ""{id}"" under the namespace ""{ns}"" is not found.");

        return command;
    }

    protected override void OnDestroy()
    {
        extensionLoader = null;
    }
}
