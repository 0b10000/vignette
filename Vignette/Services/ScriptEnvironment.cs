// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Reflection;
using Evergine.Framework.Services;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Vignette.Framework.Annotations;

namespace Vignette.Framework.Vendor;

public class ScriptEnvironment : Service
{
    static ScriptEnvironment()
    {
        HostSettings.CustomAttributeLoader = new ScriptAttributeLoader();
    }

    private V8Runtime runtime;
    private V8ScriptEngine engine;

    public V8ScriptEngine CreateEngine(string identifier, bool debuggable = false)
    {
        var flags = debuggable ? V8ScriptEngineFlags.EnableDebugging : V8ScriptEngineFlags.None;
        return runtime.CreateScriptEngine(identifier, flags, 0);
    }

    public ITypedArray<byte> CreateUint8Array(int capacity)
        => createTypedArray<byte>("Uint8Array", capacity);

    public ITypedArray<byte> CreateUint8ClampedArray(int capacity)
        => createTypedArray<byte>("Uint8ClampedArray", capacity);

    public ITypedArray<sbyte> CreateInt8Array(int capacity)
        => createTypedArray<sbyte>("Int8Array", capacity);

    public ITypedArray<ushort> CreateUint16Array(int capacity)
        => createTypedArray<ushort>("Uint16Array", capacity);

    public ITypedArray<uint> CreateUint32Array(int capacity)
        => createTypedArray<uint>("Uint32Array", capacity);

    public ITypedArray<int> CreateInt32Array(int capacity)
        => createTypedArray<int>("Int32Array", capacity);

    public ITypedArray<ulong> CreateBigUint64Array(int capacity)
        => createTypedArray<ulong>("BigUint64Array", capacity);

    public ITypedArray<long> CreateInt16Array(int capacity)
        => createTypedArray<long>("Int16Array", capacity);

    public ITypedArray<float> CreateFloat32Array(int capacity)
        => createTypedArray<float>("Float32Array", capacity);

    public ITypedArray<double> CreateFloat64Array(int capacity)
        => createTypedArray<double>("Float64Array", capacity);

    protected override void OnLoaded()
    {
        runtime = new V8Runtime(V8RuntimeFlags.EnableDynamicModuleImports);
        engine = runtime.CreateScriptEngine();
    }

    protected override void OnDestroy()
    {
        engine.Dispose();
        runtime.Dispose();
    }

    private ITypedArray<T> createTypedArray<T>(string typedArrayName, int capacity)
    {
        if (!IsLoaded || IsDestroyed)
            throw new InvalidOperationException();

        if (capacity <= 0)
            throw new ArgumentException(@"Capacity cannot be equal to or less than zero.", nameof(capacity));

        return (ITypedArray<T>)engine.Evaluate($"new {typedArrayName}({capacity})");
    }

    private class ScriptAttributeLoader : CustomAttributeLoader
    {
        public override T[] LoadCustomAttributes<T>(ICustomAttributeProvider resource, bool inherit)
        {
            if (typeof(T) == typeof(ScriptMemberAttribute) && (resource is MemberInfo member))
            {
                bool ignored = member.GetCustomAttribute<IgnoreAttribute>() == null;
                string name = char.ToLowerInvariant(member.Name[0]) + member.Name[1..];
                return new[] { new ScriptMemberAttribute(name, ignored ? ScriptAccess.None : ScriptAccess.Full) } as T[];
            }

            return base.LoadCustomAttributes<T>(resource, inherit);
        }
    }
}
