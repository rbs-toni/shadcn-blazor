namespace ShadcnBlazor.Docs;
public interface IProgressable
{
    ProgressState ProgressState { get; set; }
    string? Label { get;  }
}
