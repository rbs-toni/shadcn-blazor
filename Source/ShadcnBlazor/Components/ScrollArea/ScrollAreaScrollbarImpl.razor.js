export function setPointerCapture(id, pointerId) {
  const element = document.getElementById(id)
  if (element) {
    element.setPointerCapture(pointerId)
  }
}

export function getBoundingClientRect(element) {
  if (element) {
    return element.getBoundingClientRect()
  }
}

export function setElementStyle(element, property, value) {
  if (element) {
    element.style.setProperty(property, value)
  }
}

export function releasePointerCapture(id, pointerId) {
  const element = document.getElementById(id)
  if (element) {
    if (element.hasPointerCapture(pointerId)) {
      element.releasePointerCapture(pointerId)
    }
  }
}
let handleWheel = null
export function addWheelListener(scrollbar, dotNetHelper, method) {
  handleWheel = (event) => {
    const element = event.target
    const isScrollbarWheel = scrollbar.contains(element)
    if (isScrollbarWheel) {
      dotNetHelper.invokeMethodAsync(method, event)
    }
  }
  document.addEventListener('wheel', handleWheel)
}

export function removeWheelListener() {
  if (handleWheel) {
    document.removeEventListener('wheel', handleWheel)
    handleWheel = null
  }
}
function toInt(value) {
  return value ? Number.parseInt(value, 10) : 0
}

let prevState = null
export function addResizeObserver(viewportEl, scrollbarEl, contentEl, isHorizontal, dotnet, method) {
  if (!viewportEl || !scrollbarEl || !contentEl || !dotnet || !method)
    return null


  const areEqual = (a, b) => {
    if (!a || !b) return false
    return (
      a.content === b.content &&
      a.viewport === b.viewport &&
      a.scrollbar.size === b.scrollbar.size &&
      a.scrollbar.paddingStart === b.scrollbar.paddingStart &&
      a.scrollbar.paddingEnd === b.scrollbar.paddingEnd
    )
  }

  const handler = () => {
    const content = isHorizontal
      ? viewportEl.scrollWidth ?? 0
      : viewportEl.scrollHeight ?? 0

    const viewport = isHorizontal
      ? viewportEl.offsetWidth ?? 0
      : viewportEl.offsetHeight ?? 0

    const scrollbar = isHorizontal
      ? {
        size: scrollbarEl.clientWidth ?? 0,
        paddingStart: toInt(getComputedStyle(scrollbarEl).paddingLeft),
        paddingEnd: toInt(getComputedStyle(scrollbarEl).paddingRight)
      }
      : {
        size: scrollbarEl.clientHeight ?? 0,
        paddingStart: toInt(getComputedStyle(scrollbarEl).paddingTop),
        paddingEnd: toInt(getComputedStyle(scrollbarEl).paddingBottom)
      }

    const currentState = { content, viewport, scrollbar }

    if (areEqual(prevState, currentState))
      return

    prevState = currentState

    dotnet.invokeMethodAsync(method, currentState)
      .catch(() => { })
  }

  const scrollbarResizeObserver = new ResizeObserver(handler)
  const contentResizeObserver = new ResizeObserver(handler)

  scrollbarResizeObserver.observe(scrollbarEl)
  contentResizeObserver.observe(contentEl)

  return {
    dispose: () => {
      scrollbarResizeObserver.disconnect()
      contentResizeObserver.disconnect()
      prevState = null
    }
  }
}

export function getElementInfo(element) {
  if (element) {
    return {
      scrollWidth: element.scrollWidth,
      scrollHeight: element.scrollHeight,
      offsetWidth: element.offsetWidth,
      offsetHeight: element.offsetHeight,
      clientWidth: element.clientWidth,
      clientHeight: element.clientHeight,
    }
  }
  return null
}
export function getComputedStyle(element, prop) {
  if (element) {
    const value = toInt(window.getComputedStyle(element).getPropertyValue(prop))
    return value
  }
  return null
}