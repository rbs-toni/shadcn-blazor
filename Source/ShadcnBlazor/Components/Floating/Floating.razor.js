import { computePosition, autoUpdate, offset, flip, shift, size } from '../../modules/floating-ui.min.js'

const instances = new Map()
function getSideAndAlignFromPlacement(placement) {
  const [side, align = 'center'] = placement.split('-')
  return [side, align]
}

function transformOrigin(options) {
  return {
    name: 'transformOrigin',
    options: options,
    fn: function (data) {
      const { placement, rects, middlewareData } = data

      const cannotCenterArrow = middlewareData.arrow?.centerOffset !== 0
      const isArrowHidden = cannotCenterArrow
      const arrowWidth = isArrowHidden ? 0 : options.arrowWidth
      const arrowHeight = isArrowHidden ? 0 : options.arrowHeight

      const [placedSide, placedAlign] = getSideAndAlignFromPlacement(placement)
      const noArrowAlign = { start: '0%', center: '50%', end: '100%' }[placedAlign]

      const arrowXCenter = (middlewareData.arrow?.x || 0) + arrowWidth / 2
      const arrowYCenter = (middlewareData.arrow?.y || 0) + arrowHeight / 2

      let x = ''
      let y = ''

      if (placedSide === 'bottom') {
        x = isArrowHidden ? noArrowAlign : `${arrowXCenter}px`
        y = `${-arrowHeight}px`
      } else if (placedSide === 'top') {
        x = isArrowHidden ? noArrowAlign : `${arrowXCenter}px`
        y = `${rects.floating.height + arrowHeight}px`
      } else if (placedSide === 'right') {
        x = `${-arrowHeight}px`
        y = isArrowHidden ? noArrowAlign : `${arrowYCenter}px`
      } else if (placedSide === 'left') {
        x = `${rects.floating.width + arrowHeight}px`
        y = isArrowHidden ? noArrowAlign : `${arrowYCenter}px`
      }

      return { data: { x, y } }
    }
  }
}

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

  if (options.offset) {
    internalOptions.middleware.push(offset(options.offset))
  }

  internalOptions.middleware.push(flip())
  internalOptions.middleware.push(shift())
  internalOptions.middleware.push(transformOrigin())
  internalOptions.middleware.push(size({
    apply: ({ elements, rects, availableWidth, availableHeight }) => {
      const { width: anchorWidth, height: anchorHeight } = rects.reference
      const contentStyle = elements.floating.style
      contentStyle.setProperty('--floating-ui-available-width', `${availableWidth}px`)
      contentStyle.setProperty('--floating-ui-available-height', `${availableHeight}px`)
      contentStyle.setProperty('--floating-ui-anchor-width', `${anchorWidth}px`)
      contentStyle.setProperty('--floating-ui-anchor-height', `${anchorHeight}px`)
    },
  }))

  function updatePosition() {
    computePosition(referenceEl, floatingEl, internalOptions)
      .then(({ x, y, placement, strategy, middlewareData }) => {
        floatingEl.style.left = `${x}px`
        floatingEl.style.top = `${y}px`
        floatingEl.style.setProperty('--floating-ui-transform-origin', `${middlewareData.transformOrigin?.x}, ${middlewareData.transformOrigin?.y}`)
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

export function dispose(floatingId) {
  if (instances.has(floatingId)) {
    instances.get(floatingId).cleanup()
    instances.delete(floatingId)
  }
}
