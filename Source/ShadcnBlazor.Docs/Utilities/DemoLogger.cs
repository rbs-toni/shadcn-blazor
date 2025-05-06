using System;
using System.Linq;

namespace ShadcnBlazor.Docs;
public static class DemoLogger
{
    public static event OnLogHandler? OnLogHandler;

    public static void WriteLine(string text)
    {
        OnLogHandler?.Invoke(text);
    }
}

