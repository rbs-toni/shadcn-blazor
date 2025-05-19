const PAGE_KEYS = ['PageUp', 'PageDown']
const ARROW_KEYS = ['ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight']
export function init(el, dotnet) {
  const handlers = {
    keydown: (e) => {
      if (e.key === "Home") {
        dotnet.invokeMethodAsync("OnKeyDownHandler", { key: e.key })
        e.preventDefault()
      }
      else if (e.key === "End") {
        dotnet.invokeMethodAsync("OnKeyDownHandler", { key: e.key })
        e.preventDefault()
      }
      else if (PAGE_KEYS.concat(ARROW_KEYS).includes(e.key)) {
        dotnet.invokeMethodAsync("OnKeyDownHandler", { key: e.key })
        e.preventDefault();
      }
    },
    pointerdown: e => {
      const target = e.target;
      target.setPointerCapture(e.pointerId);
      e.preventDefault();
      dotnet.invokeMethodAsync("OnPointerDownHandler", target.id, {
        clientX: e.clientX,
        clientY: e.clientY,
        movementX: e.movementX,
        movementY: e.movementY
      })
    },
    pointermove: e => {
      const target = e.target;
      if (target.hasPointerCapture(e.pointerId)) {
        dotnet.invokeMethodAsync("OnPointerMoveHandler", {
          clientX: e.clientX,
          clientY: e.clientY,
          movementX: e.movementX,
          movementY: e.movementY
        })
      }
    },
    pointerup: e => {
      const target = e.target;
      if (target.hasPointerCapture(e.pointerId)) {
        target.releasePointerCapture(e.pointerId);
        dotnet.invokeMethodAsync("OnPointerUpHandler", {
          clientX: e.clientX,
          clientY: e.clientY,
          movementX: e.movementX,
          movementY: e.movementY
        })
      }
    }
  }
  Object.keys(handlers).forEach(event => {
    el.addEventListener(event, handlers[event])
  })

  return {
    dispose: () => {
      Object.keys(handlers).forEach(event => {
        el.removeEventListener(event, handlers[event])
      })
    }
  }
}

export function focus(id) {
  if (id) {
    const el = document.getElementById(id)
    if (el) {
      el.focus()
    }
  }
}