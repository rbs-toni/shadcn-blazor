namespace ShadcnBlazor;
public partial class Floating : ShadcnJSComponentBase
{
    FloatingOffset? _offset;
    FloatingPlacement _placement = FloatingPlacement.Bottom;
    string? _referenceId;
    FloatingStrategy _strategy = FloatingStrategy.Absolute;

    public Floating() : base("Floating/Floating")
    {
        Id = Identifier.NewId();
    }

    FloatingOptions Options => BuildOptions();
    string? StyleNames => new StyleBuilder()
        .AddStyle("position", Strategy.ToAttributeValue())
        .AddStyle(Style)
        .Build();

    protected override async ValueTask OnAfterImportAsync()
    {
        await base.OnAfterImportAsync();
        _referenceId = await InitAsync();
    }
    FloatingOptions BuildOptions()
    {
        var options = new FloatingOptions()
        {
            Placement = Placement,
            Strategy = Strategy,
            Offset = Offset,
        };

        return options;
    }
    async Task ChangeOptionsAsync()
    {
        await InvokeVoidAsync("changeOptions", Id, Options);
    }
    async Task<string> InitAsync()
    {
        return await InvokeAsync<string>("init", Anchor, Id, Options);
    }
}
