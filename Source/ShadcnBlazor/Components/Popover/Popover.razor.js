export function init(id, dotnet, method) {
  if (!document.popoverData) {
    document.popoverData = {}
  }

  if (document.popoverData[id]) {
    return
  }

  document.popoverData[id] = {
    clickHandler: async function (event) {
      const isInside = !!document.getElementById(id) && isClickInsideContainer(event, document.getElementById(id))
      if (!isInside) {
        dotnet.invokeMethodAsync(method, event)
      }
    }
  }
  document.addEventListener('click', document.popoverData[id].clickHandler)
}

export function dispose(id) {
  if (document.popoverData[id]) {
    document.removeEventListener('click', document.popoverData[id].clickHandler)
    document.popoverData[id] = null
    delete document.popoverData[id]
  }
}

function isClickInsideContainer(event, container) {
  if (!!container) {
    const rect = container.getBoundingClientRect()

    return (
      event.clientX >= rect.left &&
      event.clientX <= rect.right &&
      event.clientY >= rect.top &&
      event.clientY <= rect.bottom
    )
  }

  // Default is true
  return true
}
