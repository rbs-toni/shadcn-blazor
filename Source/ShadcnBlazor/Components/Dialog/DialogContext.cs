using System;
using System.Linq;

namespace ShadcnBlazor;
public class DialogContext
{
    private readonly Dialog _dialog;

    public DialogContext(Dialog dialog)
    {
        _dialog = dialog;
        TriggerId = Identifier.NewId();
        ContentId = Identifier.NewId();
        DescriptionId = Identifier.NewId();
    }
    public bool Open => _dialog.Open;
    public string OpenValueAsString => Open ? "open":"closed";
    public bool Modal => _dialog.Modal;
    public string TriggerId { get; }
    public string ContentId { get; }
    public string DescriptionId { get; }

    public void Toggle(bool open)
    {
        if (open)
        {
            _dialog.ShowAsync();
        }
        else
        {
            _dialog.CloseAsync();
        }
    }
}
