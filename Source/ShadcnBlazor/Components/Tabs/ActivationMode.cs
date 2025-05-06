using System;
using System.Linq;

namespace ShadcnBlazor;
public enum ActivationMode
{
    Automatic,
    Manual
}

public static class ActivationModeExtensions
{
    public static string ToStringFast(this ActivationMode mode)
    {
        return mode switch
        {
            ActivationMode.Automatic => "automatic",
            ActivationMode.Manual => "manual",
            _ => "",
        };
    }
}
