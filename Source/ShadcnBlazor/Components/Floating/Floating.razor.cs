namespace ShadcnBlazor;
public partial class Floating : ShadcnJSComponentBase
{
    FloatingOffset _offset = new(10);
    FloatingStrategy _strategy = FloatingStrategy.Fixed;

    public Floating() : base("Floating/Floating")
    {
        Id = Identifier.NewId();
    }

    FloatingOptions Options => BuildOptions();

    protected override async ValueTask OnAfterImportAsync()
    {
        await InitAsync();
    }
    protected override async ValueTask OnDisposingAsync()
    {
        await InvokeVoidAsync("dispose", Id);
    }
    FloatingOptions BuildOptions()
    {
        var placement = FloatingUtils.BuildPlacement(Side, Align);

        return new(placement ?? "bottom", Strategy, Offset);
    }
    async Task ChangeOptionsAsync() { await InvokeVoidAsync("changeOptions", Id, Options); }
    async Task<string> InitAsync()
    {
        return await InvokeAsync<string>("init", AnchorId, Id, Options);
    }
}
