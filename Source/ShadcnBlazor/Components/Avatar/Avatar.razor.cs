using System;
using System.Linq;

namespace ShadcnBlazor;
public partial class Avatar:IDisposable
{
    readonly AvatarContext _context;

    public Avatar()
    {
        _context = new AvatarContext(this);
    }

    public void Dispose()
    {
        if (GroupContext != null)
        {
            GroupContext.Remove(this);
        }
    }

    protected override void OnInitialized()
    {
        if (GroupContext != null)
        {
            GroupContext.Add(this);
        }
    }
}
