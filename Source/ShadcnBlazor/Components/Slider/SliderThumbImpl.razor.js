const map = new WeakMap()

export function useSize(el, dotnet, method) {
  // Early return if element is null or undefined
  if (!el || !(el instanceof Element)) {
    console.error('Invalid element passed to useSize:', el);
    return null;
  }

  // Check if we're already observing this element
  if (map.has(el)) {
    return null
  }

  let size = {
    width: el.offsetWidth,
    height: el.offsetHeight
  }

  const resizeObserver = new ResizeObserver((entries) => {
    if (!Array.isArray(entries) || !entries.length) {
      return
    }

    const entry = entries[0]
    let width, height

    if ('borderBoxSize' in entry) {
      const borderSizeEntry = entry.borderBoxSize
      const borderSize = Array.isArray(borderSizeEntry)
        ? borderSizeEntry[0]
        : borderSizeEntry
      width = borderSize.inlineSize
      height = borderSize.blockSize
    } else {
      // Fallback for browsers that don't support borderBoxSize
      width = el.offsetWidth
      height = el.offsetHeight
    }

    // Only notify if size actually changed
    if (size.width !== width || size.height !== height) {
      size = { width, height }
      try {
        dotnet.invokeMethodAsync(method, size)
      } catch (error) {
        console.error('Error invoking .NET method:', error)
      }
    }
  })

  // Start observing with border-box sizing
  resizeObserver.observe(el, { box: 'border-box' })

  // Store the observer in the WeakMap
  map.set(el, resizeObserver)

  return {
    dispose: () => {
      if (map.has(el)) {
        resizeObserver.unobserve(el)
        resizeObserver.disconnect()
        map.delete(el)
      }
    }
  }
}