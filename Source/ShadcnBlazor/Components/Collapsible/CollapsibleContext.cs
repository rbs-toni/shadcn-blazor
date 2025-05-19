using System;
using System.Linq;
using System.Threading.Tasks;

namespace ShadcnBlazor;
class CollapsibleContext
{
    readonly Collapsible _collapsible;

    public CollapsibleContext(Collapsible collapsible)
    {
        _collapsible = collapsible;
        ContentId = Identifier.NewId();
    }
    public string ContentId { get; }
    public bool Open => _collapsible.Open;
    public bool Disabled => _collapsible.Disabled;
    public async Task ToggleOpenAsync()
    {
        await _collapsible.ToggleOpenAsync();
    }
}
