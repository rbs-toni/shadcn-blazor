export function getBoundingClientRect(id) {
  if (id) {
    const el = document.getElementById(id)
    if (el) {
      var rect = el.getBoundingClientRect()
      return rect
    }
  }
}

export function getElementInfoAsync(id) {
  if (id) {
    const el = document.getElementById(id)
    if (el) {
      return {
        clientWidth: el.clientWidth,
        clientHeight: el.clientHeight,
      }
    }
  }
}