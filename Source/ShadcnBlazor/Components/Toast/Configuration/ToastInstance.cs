using Microsoft.AspNetCore.Components;
using System;

namespace ShadcnBlazor;
public class ToastInstance
{
    public ToastInstance()
    {
        Id = Identifier.NewId();
        TimeStamp = DateTime.Now;
    }

    public string Id { get; }
    public ToastType Type { get; set; }
    public TextOrFragment Title { get; set; }
    public TextOrFragment? Description { get; set; }
    public CustomComponent? Component { get; set; }
    public RenderFragment? Icon { get; set; }
    public bool? RichColors { get; set; }
    public bool? Invert { get; set; }
    public bool? CloseButton { get; set; }
    public bool? Dismissible { get; set; }
    public int? Duration { get; set; }
    public bool? Delete { get; set; }
    public bool? Important { get; set; }
    public ToastAction? Action { get; set; }
    public ToastAction? Cancel { get; set; }
    public EventCallback<ToastInstance>? OnDismiss { get; set; }
    public EventCallback<ToastInstance>? OnAutoClose { get; set; }
    public Func<Task>? Promise { get; set; }
    public string? CancelButtonStyle { get; set; }
    public string? ActionButtonStyle { get; set; }
    public string? Style { get; set; }
    public bool? Unstyled { get; set; }
    public string? Class { get; set; }
    public ToastClasses? Classes { get; set; }
    public string? DescriptionClass { get; set; }
    public DateTime TimeStamp { get; } = DateTime.Now;
    public ToastPosition? Position { get; set; }

    public static ToastInstance Create(string title, ToastType type, ToastParameters parameters)
    {
        return new ToastInstance
        {
            Type = type,
            Title = title,
            Description = parameters.Description,
            Icon = parameters.Icon,
            RichColors = parameters.RichColors,
            Invert = parameters.Invert,
            CloseButton = parameters.CloseButton,
            Dismissible = parameters.Dismissible,
            Duration = parameters.Duration,
            Important = parameters.Important,
            Action = parameters.Action,
            Cancel = parameters.Cancel,
            OnDismiss = parameters.OnDismiss,
            OnAutoClose = parameters.OnAutoClose,
            CancelButtonStyle = parameters.CancelButtonStyle,
            ActionButtonStyle = parameters.ActionButtonStyle,
            Style = parameters.Style,
            Unstyled = parameters.Unstyled,
            Class = parameters.Class,
            Classes = parameters.Classes ?? new ToastClasses(),
            DescriptionClass = parameters.DescriptionClass,
            Position = parameters.Position
        };
    }
}

public class ToastParameters
{
    public RenderFragment? Icon { get; set; }
    public bool? RichColors { get; set; }
    public bool? Invert { get; set; }
    public bool? CloseButton { get; set; }
    public bool? Dismissible { get; set; }
    public TextOrFragment? Description { get; set; }
    public int? Duration { get; set; }
    public bool? Important { get; set; }
    public ToastAction? Action { get; set; }
    public ToastAction? Cancel { get; set; }
    public EventCallback<ToastInstance>? OnDismiss { get; set; }
    public EventCallback<ToastInstance>? OnAutoClose { get; set; }
    public string? CancelButtonStyle { get; set; }
    public string? ActionButtonStyle { get; set; }
    public string? Style { get; set; }
    public bool? Unstyled { get; set; }
    public string? Class { get; set; }
    public ToastClasses? Classes { get; set; }
    public string? DescriptionClass { get; set; }
    public ToastPosition? Position { get; set; }
}
