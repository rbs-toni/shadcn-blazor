using System;
using System.Linq;

namespace ShadcnBlazor;
public class ProgressRootContext
{
    readonly ProgressRoot _progressRoot;

    public ProgressRootContext(ProgressRoot progressRoot)
    {
        _progressRoot = progressRoot;
    }

    public int? Max => _progressRoot.Max;
    public string? ProgressState => _progressRoot.ProgressState;
    public int? Value => _progressRoot.Value;
}
