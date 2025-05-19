let intersectingHandles = new Set()
let isPointerDown = false
let ownerDocumentCounts = new Map()
let panelConstraintFlags = new Map()
let registeredResizeHandlers = new Map()

function getInputType() {
  return window.matchMedia('(pointer:coarse)').matches ? 'coarse' : 'fine'
}

const isCoarsePointer = getInputType() === 'coarse'

export function registerResizeHandle(resizeHandleId, element, direction, hitAreaMargins, dotNetObject, method) {
  console.log(`Registering resize handle: ${resizeHandleId}`, { element, direction, hitAreaMargins });

  const ownerDocument = element.ownerDocument
  const data = {
    resizeHandleId,
    direction,
    element,
    hitAreaMargins,
    dotNetObject,
    setResizeHandlerState: (action, isActive, event) => {
      const eventData = {
        clientX: event?.clientX || 0,
        clientY: event?.clientY || 0,
        key: event?.key || null
      }
      console.log(`Invoking resize handler state: ${resizeHandleId}`, { action, isActive, eventData });
      dotNetObject.invokeMethodAsync(method, action, isActive, eventData)
    }
  }

  const count = ownerDocumentCounts.get(ownerDocument) || 0
  ownerDocumentCounts.set(ownerDocument, count + 1)
  console.log(`Owner document count updated: ${ownerDocument} = ${count + 1}`);

  registeredResizeHandlers.set(resizeHandleId, data)
  updateListeners()

  return {
    dispose: () => {
      console.log(`Disposing resize handle: ${resizeHandleId}`);
      panelConstraintFlags.delete(resizeHandleId)
      registeredResizeHandlers.delete(resizeHandleId)
      intersectingHandles.delete(resizeHandleId)

      const count = ownerDocumentCounts.get(ownerDocument) || 1
      ownerDocumentCounts.set(ownerDocument, count - 1)
      console.log(`Owner document count updated: ${ownerDocument} = ${count - 1}`);

      updateListeners()
      resetGlobalCursorStyle()

      if (count === 1) {
        ownerDocumentCounts.delete(ownerDocument)
      }
    }
  }
}

function handlePointerDown(event) {
  console.log('Pointer down event', event);
  const { target } = event
  const { x, y } = getResizeEventCoordinates(event)

  isPointerDown = true
  recalculateIntersectingHandles({ target, x, y })
  updateListeners()

  if (intersectingHandles.size > 0) {
    console.log(`Intersecting handles found: ${intersectingHandles.size}`);
    updateResizeHandlerStates('down', event)
    event.preventDefault()
  }
}

function handlePointerMove(event) {
  console.log('Pointer move event', event);
  const { x, y } = getResizeEventCoordinates(event)

  if (!isPointerDown) {
    const { target } = event
    recalculateIntersectingHandles({ target, x, y })
  }

  updateResizeHandlerStates('move', event)
  updateCursor()

  if (intersectingHandles.size > 0) {
    event.preventDefault()
  }
}

function handlePointerUp(event) {
  console.log('Pointer up event', event);
  const { target } = event
  const { x, y } = getResizeEventCoordinates(event)

  panelConstraintFlags.clear()
  isPointerDown = false

  if (intersectingHandles.size > 0) {
    event.preventDefault()
  }

  updateResizeHandlerStates('up', event)
  recalculateIntersectingHandles({ target, x, y })
  updateCursor()
  updateListeners()
}

function getResizeEventCoordinates(event) {
  if (event.touches || event.changedTouches) {
    const touch = event.touches?.[0] || event.changedTouches?.[0]
    return {
      x: touch.clientX,
      y: touch.clientY
    }
  }
  return {
    x: event.clientX,
    y: event.clientY
  }
}

function recalculateIntersectingHandles({ target, x, y }) {
  console.log(`Recalculating intersecting handles at (${x}, ${y})`);
  intersectingHandles.clear()
  let targetElement = target instanceof Element ? target : null

  registeredResizeHandlers.forEach((data, resizeHandleId) => {
    const { element: dragHandleElement, hitAreaMargins } = data
    const dragHandleRect = dragHandleElement.getBoundingClientRect()
    const { bottom, left, right, top } = dragHandleRect

    const margin = isCoarsePointer ? hitAreaMargins.coarse : hitAreaMargins.fine
    const eventIntersects =
      x >= left - margin &&
      x <= right + margin &&
      y >= top - margin &&
      y <= bottom + margin

    console.log(`Checking intersection for ${resizeHandleId}:`, {
      rect: { left, right, top, bottom },
      margin,
      eventIntersects
    });

    if (eventIntersects) {
      if (targetElement &&
        dragHandleElement !== targetElement &&
        !dragHandleElement.contains(targetElement)) {

        let currentElement = targetElement
        let didIntersect = false

        while (currentElement && currentElement !== document.body) {
          if (currentElement.contains(dragHandleElement)) break
          if (elementsIntersect(currentElement, dragHandleElement)) {
            didIntersect = true
            break
          }
          currentElement = currentElement.parentElement
        }

        if (didIntersect) {
          console.log(`Skipping ${resizeHandleId} due to element intersection`);
          return
        }
      }

      intersectingHandles.add(resizeHandleId)
      console.log(`Added ${resizeHandleId} to intersecting handles`);
    }
  })
}

function compareStackingOrder(a, b) {
  const aZIndex = parseInt(window.getComputedStyle(a).zIndex) || 0
  const bZIndex = parseInt(window.getComputedStyle(b).zIndex) || 0
  return aZIndex - bZIndex
}

function elementsIntersect(a, b) {
  const aRect = a.getBoundingClientRect()
  const bRect = b.getBoundingClientRect()
  return !(
    aRect.right < bRect.left ||
    aRect.left > bRect.right ||
    aRect.bottom < bRect.top ||
    aRect.top > bRect.bottom
  )
}

function updateCursor() {
  let intersectsHorizontal = false
  let intersectsVertical = false

  intersectingHandles.forEach(resizeHandleId => {
    const data = registeredResizeHandlers.get(resizeHandleId)
    if (data.direction === 'horizontal') {
      intersectsHorizontal = true
    } else {
      intersectsVertical = true
    }
  })

  let constraintFlags = 0
  panelConstraintFlags.forEach(flag => {
    constraintFlags |= flag
  })

  console.log('Updating cursor:', {
    intersectsHorizontal,
    intersectsVertical,
    constraintFlags: constraintFlags.toString(2)
  });

  if (intersectsHorizontal && intersectsVertical) {
    setGlobalCursorStyle('intersection', constraintFlags)
  } else if (intersectsHorizontal) {
    setGlobalCursorStyle('horizontal', constraintFlags)
  } else if (intersectsVertical) {
    setGlobalCursorStyle('vertical', constraintFlags)
  } else {
    resetGlobalCursorStyle()
  }
}

function setGlobalCursorStyle(direction, constraintFlags) {
  let cursor = direction === 'horizontal' ? 'col-resize' : 'row-resize'

  if (constraintFlags & 0b0001) cursor = 'w-resize' // EXCEEDED_HORIZONTAL_MIN
  if (constraintFlags & 0b0010) cursor = 'e-resize' // EXCEEDED_HORIZONTAL_MAX
  if (constraintFlags & 0b0100) cursor = 'n-resize' // EXCEEDED_VERTICAL_MIN
  if (constraintFlags & 0b1000) cursor = 's-resize' // EXCEEDED_VERTICAL_MAX

  console.log(`Setting global cursor to: ${cursor}`);
  document.body.style.cursor = cursor
}

function resetGlobalCursorStyle() {
  console.log('Resetting global cursor style');
  document.body.style.cursor = ''
}

function updateListeners() {
  console.log('Updating event listeners', {
    isPointerDown,
    intersectingHandles: intersectingHandles.size,
    registeredResizeHandlers: registeredResizeHandlers.size
  });

  ownerDocumentCounts.forEach((_, ownerDocument) => {
    const { body } = ownerDocument
    body.removeEventListener('contextmenu', handlePointerUp)
    body.removeEventListener('mousedown', handlePointerDown)
    body.removeEventListener('mouseleave', handlePointerMove)
    body.removeEventListener('mousemove', handlePointerMove)
    body.removeEventListener('touchmove', handlePointerMove, { passive: false })
    body.removeEventListener('touchstart', handlePointerDown)
  })

  window.removeEventListener('mouseup', handlePointerUp)
  window.removeEventListener('touchcancel', handlePointerUp)
  window.removeEventListener('touchend', handlePointerUp)

  if (registeredResizeHandlers.size > 0) {
    if (isPointerDown) {
      if (intersectingHandles.size > 0) {
        ownerDocumentCounts.forEach((count, ownerDocument) => {
          const { body } = ownerDocument
          if (count > 0) {
            console.log(`Adding active pointer listeners to ${ownerDocument}`);
            body.addEventListener('contextmenu', handlePointerUp)
            body.addEventListener('mouseleave', handlePointerMove)
            body.addEventListener('mousemove', handlePointerMove)
            body.addEventListener('touchmove', handlePointerMove, { passive: false })
          }
        })
      }

      window.addEventListener('mouseup', handlePointerUp)
      window.addEventListener('touchcancel', handlePointerUp)
      window.addEventListener('touchend', handlePointerUp)
    } else {
      ownerDocumentCounts.forEach((count, ownerDocument) => {
        const { body } = ownerDocument
        if (count > 0) {
          console.log(`Adding passive pointer listeners to ${ownerDocument}`);
          body.addEventListener('mousedown', handlePointerDown)
          body.addEventListener('mousemove', handlePointerMove)
          body.addEventListener('touchmove', handlePointerMove, { passive: false })
          body.addEventListener('touchstart', handlePointerDown)
        }
      })
    }
  }
}

function updateResizeHandlerStates(action, event) {
  registeredResizeHandlers.forEach((data, resizeHandleId) => {
    const isActive = intersectingHandles.has(resizeHandleId)
    console.log(`Updating resize handler state: ${resizeHandleId}`, { action, isActive });
    data.setResizeHandlerState(action, isActive, event)
  })
}

export function reportConstraintsViolation(resizeHandleId, flag) {
  console.log(`Reporting constraints violation: ${resizeHandleId}`, { flag });
  panelConstraintFlags.set(resizeHandleId, flag)
  updateCursor()
}