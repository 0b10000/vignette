// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Vignette.Framework.Annotations;

namespace Vignette.Framework.IO.Serialization;

public class EnumFlagConverter<T> : JsonConverter<T>
    where T : struct, Enum
{
    private static readonly T[] valid;

    static EnumFlagConverter()
    {
        valid = Enum
            .GetValues<T>()
            .Where(m =>
                typeof(T)
                    .GetMember(m.ToString())
                    .FirstOrDefault()
                    .GetCustomAttribute<IgnoreAttribute>() == null
            ).ToArray();
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int value = 0;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            if (Enum.TryParse<T>(reader.GetString(), true, out var result) && valid.Contains(result))
                value |= Convert.ToInt32(result);
        }

        return (T)Enum.ToObject(typeof(T), value);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var flag in valid)
        {
            if (value.HasFlag(flag))
                writer.WriteStringValue(Enum.GetName(flag));
        }

        writer.WriteEndArray();
    }
}
