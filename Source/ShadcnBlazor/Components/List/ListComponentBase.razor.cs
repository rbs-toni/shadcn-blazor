using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ShadcnBlazor;
/// <summary>
/// Component that provides a list of options.
/// </summary>
/// <typeparam name="TOption"></typeparam>
public abstract partial class ListComponentBase<TOption> : ShadcnInputBase<string?>, IAsyncDisposable where TOption : notnull
{
    // We cascade the _internalListContext to descendants, which in turn call it to add themselves to the options list
    internal InternalListContext<TOption> _internalListContext;
    protected readonly RenderFragment _renderOptions;
    protected TOption? _currentSelectedOption;
    protected string? _internalValue;
    protected List<TOption> _selectedOptions = [];
    const string JS_FILE = "./_content/ShadcnBlazor/Components/List/ListComponentBase.razor.js";
    bool _hasInitializedParameters;
    IJSObjectReference? _jsModule;
    bool _multiple;

    /// <summary />
    public ListComponentBase()
    {
        _internalListContext = new(this);

        Id = Identifier.NewId();

        OptionText = (item) => item?.ToString();
        OptionValue = (item) => OptionText.Invoke(item) ?? (item?.ToString());

        _renderOptions = RenderOptions;
    }

    /// <summary>
    /// Gets or sets whether using the up and down arrow keys should change focus and select immediately or that selection is done only on Enter.
    /// ⚠️ Only applicable in single select scenarios.
    /// </summary>
    [Parameter]
    public virtual bool ChangeOnEnterOnly { get; set; } = false;
    /// <summary>
    /// Gets or sets the height of the component or of the popup panel.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }
    /// <summary>
    /// Gets or sets the content source of all items to display in this list.
    /// Each item must be instantiated (cannot be null).
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption>? Items { get; set; }
    /// <summary>
    /// If true, the user can select multiple elements.
    /// ⚠️ Only available for the Select and Listbox components.
    /// </summary>
    [Parameter]
    public virtual bool Multiple { get; set; }
    /// <summary>
    /// Gets or sets the function used to determine if an option is disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, bool>? OptionDisabled { get; set; }
    /// <summary>
    /// Gets or sets the function used to determine if an option is initially selected.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, bool>? OptionSelected { get; set; }
    /// <summary>
    /// Gets or sets the template for the <see cref="ListComponentBase{TOption}.Items"/> items.
    /// </summary>
    [Parameter]
    public virtual RenderFragment<TOption>? OptionTemplate { get; set; }
    /// <summary>
    /// Gets or sets the function used to determine which text to display for each option.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, string?> OptionText { get; set; }
    /// <summary>
    /// Gets or sets the function used to determine which value to return for the selected item.
    /// Only for <see cref="Listbox{TOption}"/> and <see cref="Select{TOption}"/> components.
    /// </summary>
    [Parameter]
    public virtual Func<TOption, string?> OptionValue { get; set; }
    /// <summary>
    /// Gets or sets the selected item.
    /// ⚠️ Only available when Multiple = false.
    /// </summary>
    [Parameter]
    public virtual TOption? SelectedOption { get; set; }
    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Only available when Multiple = false.
    /// </summary>
    [Parameter]
    public virtual EventCallback<TOption?> SelectedOptionChanged { get; set; }
    /// <summary>
    /// Gets or sets an expression that identifies the bound selected options.
    /// ⚠️ Only available when Multiple = false.
    /// </summary>
    [Parameter]
    public Expression<Func<TOption>>? SelectedOptionExpression { get; set; }
    /// <summary>
    /// Gets or sets all selected items.
    /// ⚠️ Only available when Multiple = true.
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption>? SelectedOptions { get; set; }
    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Only available when Multiple = true.
    /// </summary>
    [Parameter]
    public virtual EventCallback<IEnumerable<TOption>?> SelectedOptionsChanged { get; set; }
    /// <summary>
    /// Gets or sets an expression that identifies the bound selected options.
    /// ⚠️ Only available when Multiple = true.
    /// </summary>
    [Parameter]
    public Expression<Func<IEnumerable<TOption>>>? SelectedOptionsExpression { get; set; }
    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }
    internal override bool FieldBound => Field is not null || ValueExpression is not null || ValueChanged.HasDelegate || SelectedOptionChanged.HasDelegate || SelectedOptionExpression is not null || SelectedOptionsChanged.HasDelegate || SelectedOptionsExpression is not null;
    protected string? InternalValue
    {
        get { return GetOptionValue(SelectedOption) ?? _internalValue; }
        set
        {
            if (value != null && OptionValue != null && Items != null)
            {
                var item = Items.FirstOrDefault(i => GetOptionValue(i) == value);

                if (item is not null && !Equals(item, SelectedOption))
                {
                    SelectedOption = item;

                    Value = value;
                    // Raise Changed events in another thread
                    RaiseChangedEventsAsync().ConfigureAwait(false);
                }
            }
            _internalValue = value;
        }
    }
    /// <summary />
    protected override string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();
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
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
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
            bool isSetSelectedOption = false, isSetValue = false;
            TOption? newSelectedOption = default;
            string? newValue = null;

            foreach (var parameter in parameters)
            {
                switch (parameter.Name)
                {
                    case nameof(SelectedOption):
                        isSetSelectedOption = true;
                        newSelectedOption = (TOption?)parameter.Value;
                        break;
                    case nameof(Value):
                        isSetValue = true;
                        newValue = (string?)parameter.Value;
                        break;
                    case nameof(Items):
                        if (Items is not null && OptionSelected is not null)
                        {
                            newSelectedOption = Items.FirstOrDefault(i => OptionSelected?.Invoke(i) == true);
                            newValue = GetOptionValue(newSelectedOption);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (newSelectedOption is not null || newValue is not null || Value is not null)
            {
                if (isSetSelectedOption && !Equals(_currentSelectedOption, newSelectedOption))
                {
                    if (Items != null)
                    {
                        if (Items.Contains(newSelectedOption))
                        {
                            _currentSelectedOption = newSelectedOption;
                            // Make value follow new selected option
                            Value = GetOptionValue(_currentSelectedOption);
                            await ValueChanged.InvokeAsync(Value);
                        }
                        else
                        {
                            // If the selected option is not in the list of items, reset the selected option
                            _currentSelectedOption = SelectedOption = default;
                            // and also reset the value
                            Value = null;
                            await SelectedOptionChanged.InvokeAsync(SelectedOption);
                        }
                    }
                    else
                    {
                        // If Items is null, we don't know if the selected option is in the list of items, so we just set it
                        _currentSelectedOption = newSelectedOption;
                    }
                }

                if (isSetValue && newValue is null)
                {
                    // Check if one of the Items is selected
                    if (Items is not null)
                    {
                        newSelectedOption = Items.FirstOrDefault(item => OptionSelected?.Invoke(item) == true);
                        if (newSelectedOption is not null)
                        {
                            _currentSelectedOption = SelectedOption = newSelectedOption;
                            newValue = GetOptionValue(_currentSelectedOption);
                        }
                    }

                    if (newValue is null)
                    {
                        // If the selected option is not in the list of items, reset the selected option
                        _currentSelectedOption = SelectedOption = default;

                        if (this is not Combobox<TOption>)
                        {
                            Value = null;
                            await ValueChanged.InvokeAsync(Value);
                        }
                    }
                    else
                    {
                        Value = newValue;
                        await ValueChanged.InvokeAsync(Value);
                    }
                    await SelectedOptionChanged.InvokeAsync(SelectedOption);
                }
            }
        }

        if (!_hasInitializedParameters)
        {
            if (SelectedOptionExpression is not null)
            {
                FieldIdentifier = FieldIdentifier.Create(SelectedOptionExpression);
            }
            else if (SelectedOptionChanged.HasDelegate)
            {
                FieldIdentifier = FieldIdentifier.Create(() => SelectedOption);
            }
            else if (SelectedOptionsExpression is not null)
            {
                FieldIdentifier = FieldIdentifier.Create(SelectedOptionsExpression);
            }
            else if (SelectedOptionsChanged.HasDelegate)
            {
                FieldIdentifier = FieldIdentifier.Create(() => SelectedOptions);
            }

            _hasInitializedParameters = true;
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }
    /// <summary />
    protected virtual void AddSelectedItem(TOption? item)
    {
        if (item == null)
        {
            return;
        }

        _selectedOptions.Add(item);
    }
    /// <summary />
    protected virtual bool DisabledItem(TOption item)
    {
        return Disabled;  // To allow overrides
    }
    /// <inheritdoc />
    protected override string? FormatValueAsString(string? value)
    {
        // We special-case bool values because BindConverter reserves bool conversion for conditional attributes.
        if (value is not null && typeof(TOption) == typeof(bool))
        {
            return (bool)(object)value ? "true" : "false";
        }
        else if (typeof(TOption) == typeof(bool?))
        {
            return value is not null && (bool)(object)value ? "true" : "false";
        }

        return base.FormatValueAsString(value);
    }
    protected virtual bool? GetOptionDisabled(TOption? item)
    {
        if (item != null)
        {
            if (OptionDisabled != null)
            {
                return OptionDisabled(item);
            }
        }
        return null;
    }
    /// <summary />
    protected virtual bool GetOptionSelected(TOption item)
    {
        if (Multiple)
        {

            if (_selectedOptions == null || item == null)
            {
                return false;
            }
            else if (OptionSelected != null && _selectedOptions.Contains(item))
            {
                return OptionSelected.Invoke(item);
            }

            else if (OptionValue != null && _selectedOptions != null)
            {
                foreach (var selectedItem in _selectedOptions)
                {
                    if (GetOptionValue(item) == GetOptionValue(selectedItem))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return _selectedOptions?.Contains(item) == true;
            }
        }
        else
        {
            if (OptionSelected != null)
            {
                return OptionSelected.Invoke(item);
            }
            else if (SelectedOption == null)
            {
                return false;
            }
            else if (OptionValue != null && SelectedOption != null)
            {
                return GetOptionValue(item) == GetOptionValue(SelectedOption);
            }
            else
            {
                return Equals(item, SelectedOption);
            }
        }
    }
    /// <summary />
    protected virtual string? GetOptionText(TOption? item)
    {
        if (item != null)
        {
            return OptionText.Invoke(item) ?? item.ToString();
        }
        else
        {
            return null;
        }
    }
    /// <summary />
    protected virtual string? GetOptionValue(TOption? item)
    {
        if (item != null)
        {
            return OptionValue.Invoke(item) ?? OptionText.Invoke(item) ?? item.ToString();
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
            _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JS_FILE);
        }
    }
    protected override void OnInitialized()
    {
        if (_multiple != Multiple)
        {
            if (this is not Listbox<TOption> &&
                this is not Select<TOption> &&
                this is not Autocomplete<TOption>)
            {
                throw new ArgumentException("Only Select, Listbox and Autocomplete components support multi-selection mode. ", nameof(Multiple));
            }

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

        var item = _internalListContext.Options.FirstOrDefault(i => i.Id == id);

        if (item is null)
        {
            return;
        }
        if (!ChangeOnEnterOnly || (ChangeOnEnterOnly && e.Code == "Enter"))
        {
            await item.OnClickHandlerAsync();
        }
    }
    protected override void OnParametersSet()
    {

        if (this is not Listbox<TOption> || Items is null)
        {
            if (_internalListContext.ValueChanged.HasDelegate == false)
            {
                _internalListContext.ValueChanged = ValueChanged;
            }

            if (_internalListContext.SelectedOptionChanged.HasDelegate == false)
            {
                _internalListContext.SelectedOptionChanged = SelectedOptionChanged;
            }
        }

        if (!string.IsNullOrWhiteSpace(Value) && (InternalValue is null || InternalValue != Value))
        {
            InternalValue = Value;
        }

        if (Multiple)
        {
            if (SelectedOptions?.Any() == false && _selectedOptions.Count > 0)
            {
                _selectedOptions = [];
            }

            if (SelectedOptions != null && SelectedOptions.Any() && _selectedOptions != SelectedOptions)
            {
                _selectedOptions = [.. SelectedOptions];
            }

            if (SelectedOptions == null && Items != null && OptionSelected != null && _selectedOptions != null)
            {
                _selectedOptions.AddRange(Items.Where(item => OptionSelected.Invoke(item) && !_selectedOptions.Contains(item)));
                InternalValue = GetOptionValue(_selectedOptions.FirstOrDefault());
            }
        }
        else
        {
            if (SelectedOption is null && Value is null && Items != null && OptionSelected != null)
            {
                var item = Items.FirstOrDefault(i => OptionSelected.Invoke(i));
                var value = GetOptionValue(item);
                InternalValue = value;
                if (value is not null && value != Value && ValueChanged.HasDelegate)
                {
                    ValueChanged.InvokeAsync(value);
                }
            }
        }
    }
    protected EventCallback<string> OnSelectCallback(TOption? item)
    {
        return EventCallback.Factory.Create<string>(this, (e) => OnSelectedItemChangedHandlerAsync(item));
    }
    /// <summary />
    protected virtual async Task OnSelectedItemChangedHandlerAsync(TOption? item)
    {
        if (Disabled || item == null)
        {
            return;
        }

        if (Multiple)
        {
            if (_selectedOptions.Contains(item))
            {
                RemoveSelectedItem(item);
                await RaiseChangedEventsAsync();
            }
            else
            {
                AddSelectedItem(item);
                await RaiseChangedEventsAsync();
            }
            if (!Equals(item, SelectedOption))
            {
                SelectedOption = item;
            }
        }
        else
        {
            if (!Equals(item, SelectedOption))
            {
                SelectedOption = item;
                InternalValue = GetOptionValue(item);
                await RaiseChangedEventsAsync();
            }
        }
    }
    /// <summary />
    protected virtual async Task RaiseChangedEventsAsync()
    {
        if (Multiple)
        {
            if (SelectedOptionsChanged.HasDelegate)
            {
                await SelectedOptionsChanged.InvokeAsync(_selectedOptions);
            }
        }
        else
        {
            if (SelectedOptionChanged.HasDelegate)
            {
                await SelectedOptionChanged.InvokeAsync(SelectedOption);
            }
        }

        if (FieldBound)
        {
            EditContext?.NotifyFieldChanged(FieldIdentifier);
        }

        await base.ChangeHandlerAsync(new ChangeEventArgs() { Value = InternalValue });
    }
    /// <summary />
    protected virtual bool RemoveAllSelectedItems()
    {
        _selectedOptions = [];
        return true;
    }
    /// <summary />
    protected virtual bool RemoveSelectedItem(TOption? item)
    {
        if (item == null)
        {
            return false;
        }

        return _selectedOptions.Remove(item);
    }
    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage)
        => this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);
}
