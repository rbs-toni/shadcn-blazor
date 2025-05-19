let internalViewport = null
let internalIsHorizontal
let handleScroll = null

export function init(viewport, isHorizontal, dotNetHelper) {
  if (!viewport) {
    return
  }

  // Clean up any existing listeners
  dispose()

  internalViewport = viewport
  internalIsHorizontal = isHorizontal

  let prevScrollPos = internalIsHorizontal ? internalViewport.scrollLeft : internalViewport.scrollTop

  handleScroll = () => {
    const scrollPos = internalIsHorizontal ? internalViewport.scrollLeft : internalViewport.scrollTop
    const hasScrollInDirectionChanged = prevScrollPos !== scrollPos
    if (hasScrollInDirectionChanged) {
      dotNetHelper.invokeMethodAsync('HandleScroll', true)
    }
    prevScrollPos = scrollPos
  }

  internalViewport.addEventListener('scroll', handleScroll)
}

export function dispose() {
  if (internalViewport && handleScroll) {
    internalViewport.removeEventListener('scroll', handleScroll)
    handleScroll = null
  }
  internalViewport = null
}