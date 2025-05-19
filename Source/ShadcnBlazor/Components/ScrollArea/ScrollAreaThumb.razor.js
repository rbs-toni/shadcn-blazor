let rAF = 0

export function init(node, dotNetHelper, callbackName) {
  if (!node || !dotNetHelper || !callbackName) return

  let prevPosition = { left: node.scrollLeft, top: node.scrollTop }

  const loop = () => {
    const position = { left: node.scrollLeft, top: node.scrollTop }
    const isHorizontalScroll = prevPosition.left !== position.left
    const isVerticalScroll = prevPosition.top !== position.top

    if (isHorizontalScroll || isVerticalScroll) {
      dotNetHelper.invokeMethodAsync(callbackName).catch(() => { })
    }

    prevPosition = position
    rAF = window.requestAnimationFrame(loop)
  }

  rAF = window.requestAnimationFrame(loop)
}

export function addPointerDown(element, dotnet, callback) {
  if (element) {
    const handler = (e) => {
      console.log(e.target)
      const thumb = e.target
      const thumbRect = thumb.getBoundingClientRect()
      const x = e.clientX - thumbRect.left
      const y = e.clientY - thumbRect.top
      dotnet.invokeMethodAsync(callback, e, x, y)
    }

    element.addEventListener('pointerdown', handler)

    const cleanup = () => element.removeEventListener(handler)

    return {
      cleanup
    }
  }
}

export function dispose() {
  if (rAF) {
    window.cancelAnimationFrame(rAF)
    rAF = 0
  }
}
