namespace ShadcnBlazor;
class AvatarGroupContext
{
    readonly AvatarGroup _avatarGroup;
    readonly List<Avatar> _avatars = new();

    public AvatarGroupContext(AvatarGroup avatarGroup)
    {
        _avatarGroup = avatarGroup;
    }

    public event Action? OnAvatarAdded;
    public event Action? OnAvatarRemoved;

    public int Max => _avatarGroup.Max;
    public IReadOnlyCollection<Avatar> Avatars => _avatars;
    public int Count => _avatars.Count;
    public int IndexOf(Avatar avatar) => _avatars.IndexOf(avatar);

    public bool IsOverflow(Avatar avatar)
    {
        return _avatars.IndexOf(avatar) >= Max;
    }

    public void Add(Avatar avatar)
    {
        if (avatar is null)
        {
            return;
        }
        if (!_avatars.Contains(avatar))
        {
            _avatars.Add(avatar);
            NotifyAvatarAdded();
        }
    }
    public void Remove(Avatar avatar)
    {
        if (avatar is null)
        {
            return;
        }
        _avatars.Remove(avatar);
        NotifyAvatarRemoved();
    }
    void NotifyAvatarAdded()
    {
        OnAvatarAdded?.Invoke();
    }
    void NotifyAvatarRemoved()
    {
        OnAvatarRemoved?.Invoke();
    }
}
