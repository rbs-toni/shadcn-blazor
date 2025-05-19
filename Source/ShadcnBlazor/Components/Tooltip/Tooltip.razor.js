const anchorListenersMap = new Map()

export function init(anchorId, dotnet) {
  const anchor = document.getElementById(anchorId)
  if (!anchor) {
    return
  }

  const handleClick = () => {
    dotnet.invokeMethodAsync('HandleClick')
  }

  const handleFocus = (event) => {
    const match = !event.target?.matches?.(':focus-visible')
    dotnet.invokeMethodAsync('HandleFocus', match)
  }

  const handlePointerMove = (event) => {
    if (event.pointerType === 'touch') return
    dotnet.invokeMethodAsync('HandlePointerMove')
  }

  const handlePointerLeave = () => {
    dotnet.invokeMethodAsync('HandlePointerLeave')
  }

  const handlePointerDown = () => {
    dotnet.invokeMethodAsync('HandlePointerDown')
    document.addEventListener('pointerup', handlePointerUp, { once: true })
  }

  const handlePointerUp = () => {
    setTimeout(() => {
      dotnet.invokeMethodAsync('HandlePointerUp')
    }, 1)
  }

  const handleBlur = () => {
    dotnet.invokeMethodAsync('HandleBlur')
  }

  const listeners = {
    click: handleClick,
    focus: handleFocus,
    pointermove: handlePointerMove,
    pointerleave: handlePointerLeave,
    pointerdown: handlePointerDown,
    blur: handleBlur
  }

  for (const [event, handler] of Object.entries(listeners)) {
    anchor.addEventListener(event, handler)
  }

  anchorListenersMap.set(anchorId, { anchor, listeners })
}

export function dispose(anchorId) {
  const entry = anchorListenersMap.get(anchorId)
  if (!entry) {
    return
  }

  const { anchor, listeners } = entry
  for (const [event, handler] of Object.entries(listeners)) {
    anchor.removeEventListener(event, handler)
  }

  anchorListenersMap.delete(anchorId)
}