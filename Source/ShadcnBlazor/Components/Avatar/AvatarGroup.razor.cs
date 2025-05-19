using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class AvatarGroup
{
    readonly AvatarGroupContext _context;
    public AvatarGroup()
    {
        _context = new AvatarGroupContext(this);
    }

    protected override void OnInitialized()
    {
        _context.OnAvatarAdded += () => InvokeAsync(StateHasChanged);
    }
}
