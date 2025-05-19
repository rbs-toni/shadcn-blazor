using System;
using System.Linq;

namespace ShadcnBlazor.Docs;
public enum CodeLanguage
{
    Text,
    Bash,
    CSharp,
    CSHTML,
    CSS,
    JavaScript,
    JSON,
    XML,
}

public static class CodeLanguageExtensions
{
    public static string? FastString(this CodeLanguage language)
    {
        return language switch
        {
            CodeLanguage.Bash => "bash",
            CodeLanguage.CSharp => "cs",
            CodeLanguage.CSHTML => "razor",
            CodeLanguage.CSS => "css",
            CodeLanguage.JavaScript => "js",
            CodeLanguage.JSON => "json",
            CodeLanguage.XML => "xml",
            _ => string.Empty
        };
    }
}
