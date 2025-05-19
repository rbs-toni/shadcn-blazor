// Scripts/ShadcnBlazor.js
function extPointerEventArgsCreator(event) {
  return {
    pointerId: event.pointerId,
    width: event.width,
    height: event.height,
    pressure: event.pressure,
    tiltX: event.tiltX,
    tiltY: event.tiltY,
    pointerType: event.pointerType,
    isPrimary: event.isPrimary,
    detail: event.detail,
    screenX: event.screenX,
    screenY: event.screenY,
    clientX: event.clientX,
    clientY: event.clientY,
    offsetX: event.offsetX,
    offsetY: event.offsetY,
    pageX: event.pageX,
    pageY: event.pageY,
    movementX: event.movementX,
    movementY: event.movementY,
    button: event.button,
    buttons: event.buttons,
    ctrlKey: event.ctrlKey,
    shiftKey: event.shiftKey,
    altKey: event.altKey,
    metaKey: event.metaKey,
    type: event.type,
    target: event.target.id,
    relatedTarget: event.relatedTarget?.id,
    tagName: event.target.tagName
  };
}
function extFocusEventArgsCreator(event) {
  return {
    id: event.target.id,
    tagName: event.target.tagName,
    type: event.type,
    isContained: event.currentTarget.contains(event.relatedTarget)
  };
}
var afterStartedCalled = false;
function afterWebStarted(blazor) {
  if (!afterStartedCalled) {
    registerCustomEvent(blazor);
    afterStartedCalled = true;
  }
}
function afterStarted(blazor) {
  if (!afterStartedCalled) {
    registerCustomEvent(blazor);
    afterStartedCalled = true;
  }
}
function registerCustomEvent(blazor) {
  blazor.registerCustomEventType("extpointerdown", {
    browserEventName: "pointerdown",
    createEventArgs: extPointerEventArgsCreator
  });
  blazor.registerCustomEventType("extpointerup", {
    browserEventName: "pointerup",
    createEventArgs: extPointerEventArgsCreator
  });
  blazor.registerCustomEventType("extpointermove", {
    browserEventName: "pointermove",
    createEventArgs: extPointerEventArgsCreator
  });
  blazor.registerCustomEventType("extfocus", {
    browserEventName: "focus",
    createEventArgs: extFocusEventArgsCreator
  });
  blazor.registerCustomEventType("extblur", {
    browserEventName: "blur",
    createEventArgs: extFocusEventArgsCreator
  });
}
export {
  afterStarted,
  afterWebStarted
};
