import {
  computePosition,
  autoUpdate,
  offset,
  flip,
  shift,
  size,
  hide,
  arrow
} from '../../modules/floating-ui.min.js'

import { debounce } from '../../modules/lodash-es.min.js'

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
function useSize(arrow) {
  if (arrow) {
    return {
      width: arrow.offsetWidth,
      height: arrow.offsetHeight,
    }
  }
  return {
    width: 0,
    height: 0,
  }
}
export function create(anchor, popper, options = {}, autoUpdateOptions = {}) {
  if (!anchor || !popper) {
    console.error(`Floating UI Init Error: Missing elements anchor or popper`)
    return
  }

  const { width: arrowWidth, height: arrowHeight, showArrow } = useSize(options.arrow)

  const floatingOptions = {
    placement: options.placement || 'bottom',
    strategy: options.strategy || 'absolute',
    middleware: [
      offset({
        mainAxis: (options.sideOffset || 0) + arrowHeight,
        alignmentAxis: options.alignOffset
      }),
      flip(),
      shift({
        mainAxis: true,
        crossAxis: true
      }),
      transformOrigin({
        arrowWidth,
        arrowHeight
      }),
      size({
        apply: ({ elements, rects }) => {
          const contentStyle = elements.floating.style
          contentStyle.setProperty('--popper-available-width', `${rects.floating.width}px`)
          contentStyle.setProperty('--popper-available-height', `${rects.floating.height}px`)
          contentStyle.setProperty('--popper-anchor-width', `${rects.reference.width}px`)
          contentStyle.setProperty('--popper-anchor-height', `${rects.reference.height}px`)
        }
      }),
      hide(),
      ...(options.arrow instanceof Element ? [arrow({ element: options.arrow })] : [])
    ]
  }

  let lastPosition = null
  let updateInProgress = false

  const updatePosition = () => {
    if (updateInProgress) {
      return
    }
    updateInProgress = true
    computePosition(anchor, popper, floatingOptions)
      .then(({ x, y, placement, strategy, middlewareData }) => {
        const hasChanged =
          !lastPosition ||
          x !== lastPosition.x ||
          y !== lastPosition.y ||
          placement !== lastPosition.placement ||
          strategy !== lastPosition.strategy

        if (middlewareData.hide) {
          popper.style.visibility = middlewareData.hide.anchorHidden ? 'hidden' : 'visible'
        }

        if (hasChanged) {
          popper.style.left = `${x}px`
          popper.style.top = `${y}px`

          const originX = middlewareData.transformOrigin?.x || '50%'
          const originY = middlewareData.transformOrigin?.y || '50%'

          popper.style.setProperty('--popper-transform-origin', `${originX}, ${originY}`)

          if (middlewareData.arrow) {
            const { x, y } = middlewareData.arrow
            Object.assign(options.arrow.style, {
              left: x != null ? `${x}px` : '',
              top: y != null ? `${y}px` : '',
            })
          }

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
  const debounceUpdatePosition = debounce(updatePosition, 10)
  const cleanup = autoUpdate(anchor, popper, debounceUpdatePosition, autoUpdateOptions)

  return {
    cleanup
  }
}