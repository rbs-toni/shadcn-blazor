const context = {
  layersRoot: new Set(),
  layersWithOutsidePointerEventsDisabled: new Set(),
  branches: new Set(),
  originalBodyPointerEvents: ''
}

export function initialize(dotNetObject, layerElement) {
  context.layersRoot.add(layerElement)

  // Handle escape key globally
  document.addEventListener('keydown', (event) => {
    if (event.key === 'Escape') {
      const layers = Array.from(context.layersRoot)
      const index = layers.indexOf(layerElement)

      if (index === layers.size - 1) {
        dotNetObject.invokeMethodAsync('HandleEscapeKeyDown')
      }
    }
  })
}

export function disableOutsidePointerEvents(layerElement) {
  if (context.layersWithOutsidePointerEventsDisabled.size === 0) {
    context.originalBodyPointerEvents = document.body.style.pointerEvents
    document.body.style.pointerEvents = 'none'
  }
  context.layersWithOutsidePointerEventsDisabled.add(layerElement)
}

export function handleFocusCapture(layerElement, event) {
  // Implementation for focus capture
}

export function handleBlurCapture(layerElement, event) {
  setTimeout(() => {
    if (!layerElement.contains(document.activeElement)) {
      const isFocusInBranch = Array.from(context.branches).some(branch =>
        branch && branch.contains(document.activeElement))

      if (!isFocusInBranch) {
        dotNetObject.invokeMethodAsync('HandleFocusOutside', event)
      }
    }
  }, 0)
}

export function handlePointerDownCapture(layerElement, event) {
  if (!layerElement.contains(event.target)) {
    const isPointerDownOnBranch = Array.from(context.branches).some(branch =>
      branch && branch.contains(event.target))

    const layers = Array.from(context.layersRoot)
    const index = layers.indexOf(layerElement)
    const highestLayerIndex = Array.from(context.layersWithOutsidePointerEventsDisabled)
      .map(el => layers.indexOf(el))
      .reduce((a, b) => Math.max(a, b), -1)

    const isPointerEventsEnabled = index >= highestLayerIndex

    if (isPointerEventsEnabled && !isPointerDownOnBranch) {
      dotNetObject.invokeMethodAsync('HandlePointerDownOutside', event)
    }
  }
}

export function isBodyPointerEventsDisabled() {
  return context.layersWithOutsidePointerEventsDisabled.size > 0
}

export function isPointerEventsEnabled(layerElement) {
  const layers = Array.from(context.layersRoot)
  const index = layers.indexOf(layerElement)
  const highestLayerIndex = Array.from(context.layersWithOutsidePointerEventsDisabled)
    .map(el => layers.indexOf(el))
    .reduce((a, b) => Math.max(a, b), -1)

  return index >= highestLayerIndex
}

export function cleanupLayer(layerElement) {
  context.layersRoot.delete(layerElement)
  context.layersWithOutsidePointerEventsDisabled.delete(layerElement)

  if (context.layersWithOutsidePointerEventsDisabled.size === 0) {
    document.body.style.pointerEvents = context.originalBodyPointerEvents
  }
}