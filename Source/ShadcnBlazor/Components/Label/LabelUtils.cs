using System;
using System.Linq;

namespace ShadcnBlazor;
static class LabelUtils
{
    const string DefaultClasses = "text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70";
    const string NewYorkClasses = "text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70";

    public static string GetClass(StyleType styleType)
    {
        return styleType == StyleType.Default ? DefaultClasses : NewYorkClasses;
    }
}
