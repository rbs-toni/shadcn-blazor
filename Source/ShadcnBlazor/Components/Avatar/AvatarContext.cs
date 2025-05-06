using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ShadcnBlazor;
public class AvatarContext : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    ImageLoadingStatus _imageLoadingStatus;
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
