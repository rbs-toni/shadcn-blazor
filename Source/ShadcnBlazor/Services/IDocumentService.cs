using System;
using System.Linq;

namespace ShadcnBlazor;
public interface IDocumentService
{
    Task<Direction> GetDocumentDirectionAsync();
}
