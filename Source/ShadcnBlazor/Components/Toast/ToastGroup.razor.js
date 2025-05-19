function createReactiveState(initialState, onChange) {
  return new Proxy(initialState, {
    set(target, prop, value) {
      const changed = target[prop] !== value
      target[prop] = value
      if (changed && typeof onChange === 'function') {
        onChange(prop, value, target)
      }
      return true
    }
  })
}

const noop = () => { }
export function init(el, dotnet) {
  if (!el) return { cleanUp: noop }

  // Create reactive state instance per element
  const state = createReactiveState({
    isFocusWithin: false,
    lastFocusedElement: null,
    interacting: false,
    expanded: false
  }, async (prop, value) => {
    if (prop === 'expanded') {
      await dotnet.invokeMethodAsync('InvokeOnExpanded', value)
    }
    if (prop === 'interacting') {
      await dotnet.invokeMethodAsync('InvokeOnInteracting', value)
    }
  })

  const handleMouseEnter = () => state.expanded = true
  const handleMouseMove = () => state.expanded = true
  const handleMouseLeave = () => {
    if (!state.interacting) state.expanded = false
  }

  const handleBlur = (event) => {
    if (state.isFocusWithin && !event.currentTarget.contains(event.relatedTarget)) {
      state.isFocusWithin = false
      state.lastFocusedElement?.focus({ preventScroll: true })
      state.lastFocusedElement = null
    }
  }

  const handleFocus = (event) => {
    const isNotDismissible = event.target instanceof HTMLElement &&
      event.target.dataset.dismissible === 'false'
    if (isNotDismissible) return

    if (!state.isFocusWithin) {
      state.isFocusWithin = true
      state.lastFocusedElement = event.relatedTarget
    }
  }

  const handlePointerDown = (event) => {
    const isNotDismissible = event.target instanceof HTMLElement &&
      event.target.dataset.dismissible === 'false'
    if (!isNotDismissible) state.interacting = true
  }

  const handlePointerUp = () => state.interacting = false

  // Event map
  const eventMap = {
    blur: handleBlur,
    focus: handleFocus,
    mouseenter: handleMouseEnter,
    mouseleave: handleMouseLeave,
    mousemove: handleMouseMove,
    pointerdown: handlePointerDown,
    pointerup: handlePointerUp
  }

  // Add all event listeners
  Object.entries(eventMap).forEach(([event, handler]) => {
    el.addEventListener(event, handler)
  })

  const cleanUp = () => {
    Object.entries(eventMap).forEach(([event, handler]) => {
      el.removeEventListener(event, handler)
    })
  }

  return { cleanUp }
}