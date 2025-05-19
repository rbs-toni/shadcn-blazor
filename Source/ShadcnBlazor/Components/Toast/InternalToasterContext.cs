using System;
using System.Linq;

namespace ShadcnBlazor;
public class InternalToasterContext
{
    readonly Toaster _toaster;

    public InternalToasterContext(Toaster toaster)
    {
        _toaster = toaster;
    }

    public List<ToastInstance> Toasts => _toaster.Toasts;
}
