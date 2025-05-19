let viewportResizeObserver = null
let contentResizeObserver = null

let lastState = {
  isOverflowX: null,
  isOverflowY: null
}

export function useResizeObserver(viewport, content, dotnet, method) {
  if (!viewport || !content || !dotnet || !method) return

  const handler = () => {
    const isOverflowX = viewport.offsetWidth < viewport.scrollWidth
    const isOverflowY = viewport.offsetHeight < viewport.scrollHeight

    const changed =
      lastState.isOverflowX !== isOverflowX ||
      lastState.isOverflowY !== isOverflowY

    if (changed) {
      lastState.isOverflowX = isOverflowX
      lastState.isOverflowY = isOverflowY

      dotnet.invokeMethodAsync(method, isOverflowX, isOverflowY)
        .catch(err => console.debug('[ResizeObserver] DotNet error:', err))
    }
  }

  // Create observers only once
  if (!viewportResizeObserver)
    viewportResizeObserver = new ResizeObserver(handler)

  if (!contentResizeObserver)
    contentResizeObserver = new ResizeObserver(handler)

  // Initial check
  handler()

  // Start observing both
  viewportResizeObserver.observe(viewport)
  contentResizeObserver.observe(content)
}

export function dispose() {
  viewportResizeObserver?.disconnect()
  contentResizeObserver?.disconnect()

  viewportResizeObserver = null
  contentResizeObserver = null

  lastState = {
    isOverflowX: null,
    isOverflowY: null
  }
}
