// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Evergine.Framework;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using Vignette.Framework.Extensions;

namespace Vignette.Framework.Vendor;

public abstract class VendorExtension : Extension
{
    protected ScriptEnvironment Environment { get; private set; }
    private V8ScriptEngine engine;
    private ModuleBootstrapper bootstrapper;

    protected override void OnLoad()
    {
        base.OnLoad();

        Environment = Application.Current.Container.Resolve<ScriptEnvironment>();
        engine = Environment.CreateEngine(Identifier, Mode == ExtensionMode.Development);

        bootstrapper = new ModuleBootstrapper(this, engine);
        bootstrapper.Initialize();

        var (info, code) = GetDocument();
        engine.Execute(info, code);
    }

    protected abstract Document GetDocument();

    protected override void OnDestroy()
    {
        base.OnDestroy();
        engine.Dispose();
        Environment = null;
        bootstrapper = null;
    }

    protected record Document(DocumentInfo DocumentInfo, string Code);
}
