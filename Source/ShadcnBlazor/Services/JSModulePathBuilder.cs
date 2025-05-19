using System;
using System.Linq;

namespace ShadcnBlazor;

static class JSModulePathBuilder
{
    const string BasePath = "./_content/ShadcnBlazor/modules/";

    public static string GetModulePath(string moduleName, bool minified = false)
    {
        var suffix = minified ? ".min.js" : ".js";
        return BasePath + moduleName + suffix;
    }

    public static string GetComponentPath(string folderName, string componentName)
    {
        return folderName + '/' + componentName;
    }
}
