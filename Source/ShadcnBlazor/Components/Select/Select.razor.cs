using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace ShadcnBlazor;
public partial class Select<TItem> : ShadcnInputBase<string?>, IAsyncDisposable where TItem : notnull
{
    internal InternalSelectContext<TItem> _internalSelectContext;
    protected readonly RenderFragment _renderItems;
    protected TItem? _currentSelectedItem;
    protected string? _internalValue;
    protected List<TItem> _selectedItems = [];
    const string JSFile = "./_content/ShadcnBlazor/Components/Select/Select.razor.js";
    bool _hasInitializedParameters;
    bool _multiple;

    public Select()
    {
        _internalSelectContext = new(this);

        Id = Identifier.NewId();

        ItemText = (item) => item?.ToString();
        ItemValue = (item) => ItemText.Invoke(item) ?? (item?.ToString());

        _renderItems = RenderItems;
    }

    internal override bool FieldBound
    {
        get
        {
            return Field is not null ||
                ValueExpression is not null ||
                ValueChanged.HasDelegate ||
                SelectedItemChanged.HasDelegate ||
                SelectedItemExpression is not null ||
                SelectedItemsChanged.HasDelegate ||
                SelectedItemsExpression is not null;
        }
    }

    protected string? InternalValue
    {
        get { return GetItemValue(SelectedItem) ?? _internalValue; }
        set
        {
            if (value != null && ItemValue != null && Items != null)
            {
                var item = Items.FirstOrDefault(i => GetItemValue(i) == value);

                if (item is not null && !Equals(item, SelectedItem))
                {
                    SelectedItem = item;

                    Value = value;
                    // Raise Changed events in another thread
                    RaiseChangedEventsAsync().ConfigureAwait(false);
                }
            }
            _internalValue = value;
        }
    }

    protected override string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();

    IJSObjectReference? _jsModule { get; set; }

    [Inject] IJSRuntime JSRuntime { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException || ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        if (!Multiple)
        {
            bool isSetSelectedItem = false, isSetValue = false;
            TItem? newSelectedItem = default;
            string? newValue = null;

            foreach (var parameter in parameters)
            {
                switch (parameter.Name)
                {
                    case nameof(SelectedItem):
                        isSetSelectedItem = true;
                        newSelectedItem = (TItem?)parameter.Value;
                        break;
                    case nameof(Value):
                        isSetValue = true;
                        newValue = (string?)parameter.Value;
                        break;
                    case nameof(Items):
                        if (Items is not null && ItemSelected is not null)
                        {
                            newSelectedItem = Items.FirstOrDefault(i => ItemSelected?.Invoke(i) == true);
                            newValue = GetItemValue(newSelectedItem);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (newSelectedItem is not null || newValue is not null || Value is not null)
            {
                if (isSetSelectedItem && !Equals(_currentSelectedItem, newSelectedItem))
                {
                    if (Items != null)
                    {
                        if (Items.Contains(newSelectedItem))
                        {
                            _currentSelectedItem = newSelectedItem;
                            // Make value follow new selected option
                            Value = GetItemValue(_currentSelectedItem);
                            await ValueChanged.InvokeAsync(Value);
                        }
                        else
                        {
                            // If the selected option is not in the list of items, reset the selected option
                            _currentSelectedItem = SelectedItem = default;
                            // and also reset the value
                            Value = null;
                            await SelectedItemChanged.InvokeAsync(SelectedItem);
                        }
                    }
                    else
                    {
                        // If Items is null, we don't know if the selected option is in the list of items, so we just set it
                        _currentSelectedItem = newSelectedItem;
                    }
                }

                if (isSetValue && newValue is null)
                {
                    // Check if one of the Items is selected
                    if (Items is not null)
                    {
                        newSelectedItem = Items.FirstOrDefault(item => ItemSelected?.Invoke(item) == true);
                        if (newSelectedItem is not null)
                        {
                            _currentSelectedItem = SelectedItem = newSelectedItem;
                            newValue = GetItemValue(_currentSelectedItem);
                        }
                    }

                    if (newValue is null)
                    {
                        // If the selected option is not in the list of items, reset the selected option
                        _currentSelectedItem = SelectedItem = default;

                        Value = null;
                        await ValueChanged.InvokeAsync(Value);
                    }
                    else
                    {
                        Value = newValue;
                        await ValueChanged.InvokeAsync(Value);
                    }
                    await SelectedItemChanged.InvokeAsync(SelectedItem);
                }
            }
        }

        if (!_hasInitializedParameters)
        {
            if (SelectedItemExpression is not null)
            {
                FieldIdentifier = FieldIdentifier.Create(SelectedItemExpression);
            }
            else if (SelectedItemChanged.HasDelegate)
            {
                FieldIdentifier = FieldIdentifier.Create(() => SelectedItem);
            }
            else if (SelectedItemsExpression is not null)
            {
                FieldIdentifier = FieldIdentifier.Create(SelectedItemsExpression);
            }
            else if (SelectedItemsChanged.HasDelegate)
            {
                FieldIdentifier = FieldIdentifier.Create(() => SelectedItems);
            }

            _hasInitializedParameters = true;
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }

    protected virtual void AddSelectedItem(TItem? item)
    {
        if (item == null)
        {
            return;
        }

        _selectedItems.Add(item);
    }

    protected virtual bool DisabledItem(TItem item)
    {
        return Disabled;  // To allow overrides
    }

    protected override string? FormatValueAsString(string? value)
    {
        // We special-case bool values because BindConverter reserves bool conversion for conditional attributes.
        if (value is not null && typeof(TItem) == typeof(bool))
        {
            return (bool)(object)value ? "true" : "false";
        }
        else if (typeof(TItem) == typeof(bool?))
        {
            return value is not null && (bool)(object)value ? "true" : "false";
        }

        return base.FormatValueAsString(value);
    }

    protected virtual bool? GetItemDisabled(TItem? item)
    {
        if (item != null)
        {
            if (ItemDisabled != null)
            {
                return ItemDisabled(item);
            }
        }
        return null;
    }

    protected virtual bool GetItemSelected(TItem item)
    {
        if (Multiple)
        {
            if (_selectedItems == null || item == null)
            {
                return false;
            }
            else if (ItemSelected != null && _selectedItems.Contains(item))
            {
                return ItemSelected.Invoke(item);
            }

            else if (ItemValue != null && _selectedItems != null)
            {
                foreach (var selectedItem in _selectedItems)
                {
                    if (GetItemValue(item) == GetItemValue(selectedItem))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return _selectedItems?.Contains(item) == true;
            }
        }
        else
        {
            if (ItemSelected != null)
            {
                return ItemSelected.Invoke(item);
            }
            else if (SelectedItem == null)
            {
                return false;
            }
            else if (ItemValue != null && SelectedItem != null)
            {
                return GetItemValue(item) == GetItemValue(SelectedItem);
            }
            else
            {
                return Equals(item, SelectedItem);
            }
        }
    }

    protected virtual string? GetItemText(TItem? item)
    {
        if (item != null)
        {
            return ItemText.Invoke(item) ?? item.ToString();
        }
        else
        {
            return null;
        }
    }

    protected virtual string? GetItemValue(TItem? item)
    {
        if (item != null)
        {
            return ItemValue.Invoke(item) ?? ItemText.Invoke(item) ?? item.ToString();
        }
        else
        {
            return null;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JSFile);
        }
    }

    protected override void OnInitialized()
    {
        if (_multiple != Multiple)
        {
            _multiple = Multiple;
        }

        if (!string.IsNullOrEmpty(Height) && string.IsNullOrEmpty(Id))
        {
            Id = Identifier.NewId();
        }
    }

    protected virtual async Task OnKeydownHandlerAsync(KeyboardEventArgs e)
    {
        if (e is null || Multiple)
        {
            return;
        }
        if (e.ShiftKey == true || e.AltKey == true || e.CtrlKey == true)
        {
            return;
        }
        // This delay is needed for WASM to be able to get the updated value of the active descendant.
        // Without it, the value sometimes lags behind and you then need two keypresses to move to the next/prev option.
        await Task.Delay(1);
        var id = await _jsModule!.InvokeAsync<string>("getAriaActiveDescendant", Id);

        var item = _internalSelectContext.Items.FirstOrDefault(i => i.Id == id);

        if (item is null)
        {
            return;
        }
        if (!ChangeOnEnterOnly || (ChangeOnEnterOnly && e.Code == "Enter"))
        {
            //await item.OnClickHandlerAsync();
        }
    }

    protected override void OnParametersSet()
    {
        if (Items is null)
        {
            if (!_internalSelectContext.ValueChanged.HasDelegate)
            {
                _internalSelectContext.ValueChanged = ValueChanged;
            }

            if (!_internalSelectContext.SelectedItemChanged.HasDelegate)
            {
                _internalSelectContext.SelectedItemChanged = SelectedItemChanged;
            }
        }

        if (!string.IsNullOrWhiteSpace(Value) && (InternalValue is null || InternalValue != Value))
        {
            InternalValue = Value;
        }

        if (Multiple)
        {
            if (SelectedItems?.Any() == false && _selectedItems.Count > 0)
            {
                _selectedItems = [];
            }

            if (SelectedItems != null && SelectedItems.Any() && _selectedItems != SelectedItems)
            {
                _selectedItems = [.. SelectedItems];
            }

            if (SelectedItems == null && Items != null && ItemSelected != null && _selectedItems != null)
            {
                _selectedItems.AddRange(
                    Items.Where(item => ItemSelected.Invoke(item) && !_selectedItems.Contains(item)));
                InternalValue = GetItemValue(_selectedItems.FirstOrDefault());
            }
        }
        else
        {
            if (SelectedItem is null && Value is null && Items != null && ItemSelected != null)
            {
                var item = Items.FirstOrDefault(i => ItemSelected.Invoke(i));
                var value = GetItemValue(item);
                InternalValue = value;
                if (value is not null && value != Value && ValueChanged.HasDelegate)
                {
                    ValueChanged.InvokeAsync(value);
                }
            }
        }
    }

    protected EventCallback<string> OnSelectCallback(TItem? item)
    { return EventCallback.Factory.Create<string>(this, (e) => OnSelectedItemChangedHandlerAsync(item)); }

    protected virtual async Task OnSelectedItemChangedHandlerAsync(TItem? item)
    {
        if (Disabled || item == null)
        {
            return;
        }

        if (Multiple)
        {
            if (_selectedItems.Contains(item))
            {
                RemoveSelectedItem(item);
                await RaiseChangedEventsAsync();
            }
            else
            {
                AddSelectedItem(item);
                await RaiseChangedEventsAsync();
            }
            if (!Equals(item, SelectedItem))
            {
                SelectedItem = item;
            }
        }
        else
        {
            if (!Equals(item, SelectedItem))
            {
                SelectedItem = item;
                InternalValue = GetItemValue(item);
                await RaiseChangedEventsAsync();
            }
        }
    }

    protected virtual async Task RaiseChangedEventsAsync()
    {
        if (Multiple)
        {
            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(_selectedItems);
            }
        }
        else
        {
            if (SelectedItemChanged.HasDelegate)
            {
                await SelectedItemChanged.InvokeAsync(SelectedItem);
            }
        }

        if (FieldBound)
        {
            EditContext?.NotifyFieldChanged(FieldIdentifier);
        }

        await base.ChangeHandlerAsync(new ChangeEventArgs() { Value = InternalValue });
    }

    protected virtual bool RemoveAllSelectedItems()
    {
        _selectedItems = [];
        return true;
    }

    protected virtual bool RemoveSelectedItem(TItem? item)
    {
        if (item == null)
        {
            return false;
        }

        return _selectedItems.Remove(item);
    }

    protected override bool TryParseValueFromString(
        string? value,
        [MaybeNullWhen(false)] out string? result,
        [NotNullWhen(false)] out string? validationErrorMessage) => this.TryParseSelectableValueFromString(
        value,
        out result,
        out validationErrorMessage);
}
