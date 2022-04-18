// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Vignette.Framework.Annotations;
using Vignette.Framework.Extensions;

namespace Vignette.Framework.Commands;

/// <summary>
/// Represents an invokable command.
/// </summary>
public abstract class Command : ExtensionComponent
{
    /// <summary>
    /// The identifiable name for this command.
    /// </summary>
    [Ignore]
    public abstract string Name { get; }

    /// <summary>
    /// The namespace the command is under. Usually the owning extension's identifier.
    /// </summary>
    [Ignore]
    public string Namespace => Extension.Identifier;

    /// <summary>
    /// The parameters for this command allowing it to be invokable using <see cref="Invoke(string)"/> and overloads.
    /// </summary>
    public virtual IReadOnlyList<Parameter> Parameters { get; } = Array.Empty<Parameter>();

    protected Command(Extension extension)
        : base(extension)
    {
    }

    /// <summary>
    /// Invokes the command. Arguments passed must be in order according to <see cref="Parameters"/>
    /// </summary>
    /// <exception cref="ArgumentException"/>
    /// <exception cref="InvalidOperationException"/>
    public Task<object> InvokeAsync(object[] args, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();

        if (!IsLoaded || IsDestroyed)
            throw new InvalidOperationException(@"Cannot invoke command on an unloaded or destroyed component.");

        return Perform(ensureValues(args), token);
    }

    /// <inheritdoc cref="InvokeAsync(object[], CancellationToken)"/>
    public async Task<T> InvokeAsync<T>(object[] args, CancellationToken token = default)
        => (T)await InvokeAsync(args, token);

    /// <inheritdoc cref="InvokeAsync(object[], CancellationToken)"/>
    public object Invoke(object[] args)
        => InvokeAsync(args).GetAwaiter().GetResult();

    /// <inheritdoc cref="InvokeAsync(object[], CancellationToken)"/>
    public T Invoke<T>(object[] args)
        => InvokeAsync<T>(args).GetAwaiter().GetResult();

    /// <inheritdoc cref="InvokeAsync(object[], CancellationToken)"/>
    public Task<object> InvokeAsync(string args, CancellationToken token = default)
    {
        if (Parameters.Count == 0)
            throw new InvalidOperationException(@"Command does not support parsing string arguments.");

        return InvokeAsync(parse(args), token);
    }

    /// <inheritdoc cref="InvokeAsync(object[], CancellationToken)"/>
    public async Task<T> InvokeAsync<T>(string args, CancellationToken token = default)
        => (T)await InvokeAsync(args, token);

    /// <inheritdoc cref="InvokeAsync(object[], CancellationToken)"/>
    public object Invoke(string args)
        => InvokeAsync(args).GetAwaiter().GetResult();

    /// <inheritdoc cref="InvokeAsync(object[], CancellationToken)"/>
    public T Invoke<T>(string args)
        => InvokeAsync<T>(args).GetAwaiter().GetResult();

    /// <summary>
    /// The compiled regex for command parameter parsing.
    /// </summary>
    public static readonly Regex PARAMETER_REGEX = new Regex(@"-(?<Key>\w+) (?<Value>([""'])(?:(?=(\\?))\2.)*?\1|[\w+\.]+)", RegexOptions.Compiled | RegexOptions.ECMAScript);

    /// <summary>
    /// The compiled regex for command identifier parsing.
    /// </summary>
    public static readonly Regex IDENTIFIER_REGEX = new Regex(@"(?<Namespace>\w+):(?<Name>[\w._]+)", RegexOptions.Compiled | RegexOptions.ECMAScript);

    protected abstract Task<object> Perform(object[] args, CancellationToken token = default);

    /// <summary>
    /// Parses a string input to an object array using <see cref="Parameters"/> to parse string values.
    /// </summary>
    private object[] parse(string input)
    {
        var values = new object[Parameters.Count];

        if (!string.IsNullOrEmpty(input))
        {
            var matches = PARAMETER_REGEX.Matches(input);

            foreach (Match match in matches)
            {
                string paramName = match.Groups["Key"].Value;
                string paramValue = match.Groups["Value"].Value;

                var param = Parameters.FirstOrDefault(p => p.Name.ToLower() == paramName.ToLower());

                if (param != null)
                {
                    values[param.Order] = param.Parse(paramValue);
                }
            }
        }

        return ensureValues(values);
    }

    /// <summary>
    /// Ensures that all default values are applied or throws an exception if a required value is not supplied.
    /// </summary>\
    private object[] ensureValues(object[] values)
    {
        if (Parameters.Count == 0)
            return values;

        if (values.Length != Parameters.Count)
            Array.Resize(ref values, Parameters.Count);

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] == null)
            {
                if (Parameters[i].Required)
                    throw new ArgumentException($@"Parameter ""{Parameters[i].Name}"" has no value passed.");

                values[i] = Parameters[i].Default;
            }
        }

        return values;
    }
}
