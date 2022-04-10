// Copyright (c) The Vignette Authors
// Licensed under GPL-3.0 (With SDK Exception). See LICENSE for details.

using Evergine.Common.Graphics;
using Evergine.Forms;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Vignette.Platform;

namespace Vignette.Windows;

public class Program
{
    public static void Main()
    {
        using var host = new WindowsHost();
        host.Run(new Game());
    }
}

public class WindowsHost : DesktopHost
{
    protected new FormsWindow Window => (FormsWindow)base.Window;

    protected override WindowsSystem CreateWindowsSystem()
        => new FormsWindowsSystem();

    protected override void Prepare()
    {
        var windowId = Win32Interop.GetWindowIdFromWindow(Window.NativeWindow.Handle);
        var appWindow = AppWindow.GetFromWindowId(windowId);

        appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
        appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        appWindow.TitleBar.ButtonHoverBackgroundColor = Colors.Transparent;
        appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        appWindow.TitleBar.ButtonPressedBackgroundColor = Colors.Transparent;
    }
}
