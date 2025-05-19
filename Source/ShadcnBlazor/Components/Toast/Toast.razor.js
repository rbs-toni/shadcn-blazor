let callback = null

export function init(dotnet, method) {
  console.log('listenVisibilityChange')
  callback = () => {
    dotnet.invokeMethodAsync(method, document.hidden)
  }

  document.addEventListener('visibilitychange', callback)
}

export function dispose() {
  if (callback) {
    document.removeEventListener('visibilitychange', callback)
  }
}

export function setElementPointerCapture(el, pointerId) {
  el.setPointerCapture(pointerId)
}
export function getWindowSelection() {
  return window.getSelection()?.toString().length > 0
}

export function getSwipeAmount(el) {
  const swipeAmount = Number(
    el.style
      .getPropertyValue('--swipe-amount')
      .replace('px', '') || 0
  )

  return swipeAmount
}