using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ShadcnBlazor;
class AvatarContext : INotifyPropertyChanged
{
    public AvatarContext(Avatar avatar)
    {
        _avatar = avatar;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    ImageLoadingStatus _imageLoadingStatus;
    private readonly Avatar _avatar;

    public ImageLoadingStatus ImageLoadingStatus
    {
        get => _imageLoadingStatus;
        set
        {
            if (_imageLoadingStatus != value)
            {
                _imageLoadingStatus = value;
                OnPropertyChanged();
            }
        }
    }

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = default)
            => PropertyChanged?.Invoke(this, new(propertyName));
}
