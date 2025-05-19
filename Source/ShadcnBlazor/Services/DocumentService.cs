using Microsoft.JSInterop;
using System;
using System.Linq;

namespace ShadcnBlazor;

sealed class DocumentService : JSModule, IDocumentService
{
    public DocumentService(IJSRuntime js) : base(js, JSModulePathBuilder.GetModulePath("document", true))
    {
    }

    public async Task<Direction> GetDocumentDirectionAsync()
    {
        var result = await InvokeAsync<string>("getDocumentAttribute", "dir");

        switch (result)
        {
            case "rtl":
                return Direction.LTR;
            case "ltr":
                return Direction.RTL;
            default:
                return Direction.LTR;
        }
    }
}
