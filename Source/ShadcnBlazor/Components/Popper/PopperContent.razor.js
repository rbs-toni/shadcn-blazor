import {
  computePosition,
  autoUpdate,
  offset,
  flip,
  shift,
  size,
  hide
} from '../../modules/floating-ui.min.js'

const instances = new Map()
let isPositioned = false
const callback = (dotnet) => {
  dotnet.invokeMetodAsync('UpdatePosition')
}
function getSideAndAlignFromPlacement(placement) {
  const [side, align = 'center'] = placement.split('-')
  return [side, align]
}

function transformOrigin(options) {
  return {
    name: 'transformOrigin',
    options: options,
    fn: function ({ placement, rects, middlewareData }) {
      const cannotCenterArrow = middlewareData.arrow?.centerOffset !== 0
      const isArrowHidden = cannotCenterArrow
      const arrowWidth = isArrowHidden ? 0 : options.arrowWidth
      const arrowHeight = isArrowHidden ? 0 : options.arrowHeight

      const [side, align] = getSideAndAlignFromPlacement(placement)
      const noArrowAlign = { start: '0%', center: '50%', end: '100%' }[align]

      const arrowX = (middlewareData.arrow?.x || 0) + arrowWidth / 2
      const arrowY = (middlewareData.arrow?.y || 0) + arrowHeight / 2

      const x = ['top', 'bottom'].includes(side)
        ? isArrowHidden ? noArrowAlign : `${arrowX}px`
        : side === 'left' ? `${rects.floating.width + arrowHeight}px` : `${-arrowHeight}px`

      const y = ['left', 'right'].includes(side)
        ? isArrowHidden ? noArrowAlign : `${arrowY}px`
        : side === 'top' ? `${rects.floating.height + arrowHeight}px` : `${-arrowHeight}px`

      return { data: { x, y } }
    }
  }
}

export function init(reference, popper, options = {}) {
  if (!reference || !popper) {
    console.error(`Floating UI Init Error: Missing elements reference or popper`)
    return
  }

  const {
    autoUpdateOptions
  } = options

  const internalOptions = {
    placement: options.placement || 'bottom',
    strategy: options.strategy || 'absolute',
    middleware: [
      ...(options.offset ? [offset(options.offset)] : []),
      flip(),
      shift(),
      transformOrigin(options),
      size({
        apply: ({ elements, rects }) => {
          elements.floating.style.setProperty('--blazor-popper-available-width', `${rects.floating.width}px`)
          elements.floating.style.setProperty('--blazor-popper-available-height', `${rects.floating.height}px`)
          elements.floating.style.setProperty('--blazor-popper-anchor-width', `${rects.reference.width}px`)
          elements.floating.style.setProperty('--blazor-popper-anchor-height', `${rects.reference.height}px`)
        }
      }),
      hide(),
    ]
  }

  let lastPosition = null
  let updateInProgress = false

  const updatePosition = () => {
    if (updateInProgress) {
      return
    }
    updateInProgress = true
    computePosition(referenceEl, floatingEl, internalOptions)
      .then(({ x, y, placement, strategy, middlewareData }) => {
        const hasChanged =
          !lastPosition ||
          x !== lastPosition.x ||
          y !== lastPosition.y ||
          placement !== lastPosition.placement ||
          strategy !== lastPosition.strategy

        if (middlewareData.hide) {
          Object.assign(floatingEl.style, {
            visibility: middlewareData.hide.referenceHidden
              ? 'hidden'
              : 'visible',
          })
        }

        if (!lastPosition) {
          floatingEl.style.visibility = 'visible';
        }

        if (hasChanged) {
          floatingEl.style.left = `${x}px`
          floatingEl.style.top = `${y}px`
          floatingEl.style.setProperty('--blazor-popper-transform-origin',
            `${middlewareData.transformOrigin?.x}, ${middlewareData.transformOrigin?.y}`)

          lastPosition = { x, y, placement, strategy }
        }
      })
      .catch(error => {
        console.error('Position computation failed:', error)
      })
      .finally(() => {
        updateInProgress = false
      })
  }

  const cleanup = autoUpdate(referenceEl, floatingEl, updatePosition, autoUpdateOptions)

  return cleanup
}

export function changeOptions(floatingId, options) {
  const instance = instances.get(floatingId)
  if (!instance) {
    console.error(`Floating UI Error: No instance found for ID ${floatingId}`)
    return
  }
  instance.updatePosition()
}

export function dispose(floatingId) {
  const instance = instances.get(floatingId)
  if (instance) instance.cleanup()
}