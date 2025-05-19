using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace ShadcnBlazor;
public class LibraryConfiguration
{
    internal static readonly string? AssemblyVersion = typeof(LibraryConfiguration).Assembly.GetName().Version?.ToString();

    public MarkupString RequiredLabel { get; set; } = (MarkupString)"<span aria-label=\"required\" aria-hidden=\"true\" class=\"ml-0.5 after:text-red-500 pointer-events-none\">*</span>";
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
}
