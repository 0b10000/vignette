// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Vignette.Framework.Annotations;

namespace Vignette.Framework.Vendor;

public class ModuleBootstrapper
{
    public static readonly string EXTENSION_GLOBAL_KEY = "__extension__";

    private readonly VendorExtension extension;
    private readonly V8ScriptEngine engine;
    private readonly Dictionary<string, object> globals = new Dictionary<string, object>();
    private readonly Dictionary<string, ModuleDocument> documents = new Dictionary<string, ModuleDocument>();

    public ModuleBootstrapper(VendorExtension extension, V8ScriptEngine engine)
    {
        this.engine = engine;
        this.extension = extension;
    }

    public void Initialize()
    {
        engine.Global.SetProperty(EXTENSION_GLOBAL_KEY, extension);

        foreach (var type in module_types)
        {
            var component = (Module)Activator.CreateInstance(type, extension);

            switch (component.Type)
            {
                case Module.ModuleType.Document:
                    addToDocument("vignette", component);
                    break;

                case Module.ModuleType.Global:
                    addToGlobal(component);
                    break;
            }
        }

        foreach ((string name, object global) in globals)
            engine.AddHostObject(name, global);

        foreach ((string name, ModuleDocument module) in documents)
            engine.DocumentSettings.AddSystemDocument(name, ModuleCategory.Standard, $"export default import.meta.{name}", module);
    }

    private void addToDocument(string moduleName, Module component)
    {
        string propName = camelize(component.MemberName);

        if (!documents.TryGetValue(moduleName, out var module))
        {
            module = new ModuleDocument(moduleName);
            documents.Add(moduleName, module);
        }

        module.Add(propName, component);
    }

    private void addToGlobal(Module component)
    {
        string name = camelize(component.MemberName);

        if (globals.ContainsKey(name))
            throw new ArgumentException(@"Global already exists.", nameof(component));

        globals.Add(name, component);
    }

    private static readonly string module_ns = "Vignette.Framework.Vendor.Modules";
    private static readonly Type[] module_types = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(Module)) && t.Namespace == module_ns)
        .ToArray();

    private static string camelize(string text) => char.ToLowerInvariant(text[0]) + text[1..];

    private class ModuleDocument : DynamicHostObject
    {
        [Ignore]
        public readonly string Name;
        private readonly Dictionary<string, object> container = new Dictionary<string, object>();

        public ModuleDocument(string name)
        {
            Name = name;
        }

        [Ignore]
        public void Add(string name, object obj)
            => container.Add(name, obj);

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes.Length != 1)
            {
                result = null;
                return false;
            }

            return container.TryGetValue(indexes[0].ToString(), out result);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
            => container.TryGetValue(binder.Name, out result);

        public static implicit operator DocumentContextCallback(ModuleDocument module)
        {
            return _ => new Dictionary<string, object> { { module.Name, module } };
        }
    }
}
