import { computePosition, autoUpdate, offset } from '../../modules/floating-ui.min.js'

const instances = new Map()

export function init(referenceId, floatingId, options = {}) {
  const referenceEl = document.getElementById(referenceId)
  const floatingEl = document.getElementById(floatingId)
  if (!referenceEl) {
    console.error(`Floating UI Init Error: Missing element(s) - Reference: ${!!referenceEl}`)
    return
  }

  if (!floatingEl) {
    console.error(`Floating UI Init Error: Missing element(s) - Floating: ${!!floatingEl}`)
    return
  }

  const internalOptions = {
    placement: options.placement || 'bottom',
    strategy: options.strategy || 'absolute',
    middleware: []
  }
  console.log(options)
  if (options.offset) {
    internalOptions.middleware.push(offset(options.offset))
  }

  function updatePosition() {
    computePosition(referenceEl, floatingEl, internalOptions)
      .then(({ x, y }) => {
        floatingEl.style.left = `${x}px`
        floatingEl.style.top = `${y}px`
      })
      .catch(console.error)
  }

  const cleanup = autoUpdate(referenceEl, floatingEl, updatePosition)
  instances.set(floatingId, { cleanup, referenceEl, floatingEl })

  return referenceEl.id
}

export function changeOptions(floatingId, options) {
  if (!instances.has(floatingId)) {
    console.error(`Floating UI Error: No instance found for ID ${floatingId}`)
    return
  }

  const instance = instances.get(floatingId)

  const internalOptions = {
    placement: options.placement || 'bottom',
    strategy: options.strategy || 'absolute',
    middleware: []
  }

  if (options.offset) {
    internalOptions.middleware.push(offset(options.offset))
  }

  computePosition(instance.referenceEl, instance.floatingEl, internalOptions)
    .then(({ x, y }) => {
      instance.floatingEl.style.left = `${x}px`
      instance.floatingEl.style.top = `${y}px`
    })
    .catch(console.error)
}

export function destroyFloating(floatingId) {
  if (instances.has(floatingId)) {
    instances.get(floatingId).cleanup()
    instances.delete(floatingId)
  }
}
