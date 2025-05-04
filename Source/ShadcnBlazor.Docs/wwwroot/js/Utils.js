export function debounce(func, delay) {
  let debounceTimer
  return function (...args) {
    const context = this
    clearTimeout(debounceTimer)
    debounceTimer = setTimeout(() => func.apply(context, args), delay)
  }
}

export function throttle(func, limit) {
  let lastFunc
  let lastRan
  return function (...args) {
    const context = this
    if (!lastRan) {
      func.apply(context, args)
      lastRan = Date.now()
    } else {
      clearTimeout(lastFunc)
      lastFunc = setTimeout(() => {
        if (Date.now() - lastRan >= limit) {
          func.apply(context, args)
          lastRan = Date.now()
        }
      }, limit - (Date.now() - lastRan))
    }
  }
}

export function throttleAndDebounce(fn, delay) {
  let timeoutId
  let called = false

  return () => {
    if (timeoutId) {
      clearTimeout(timeoutId)
    }

    if (!called) {
      fn()
      called = true
      setTimeout(() => {
        called = false
      }, delay)
    } else {
      timeoutId = setTimeout(fn, delay)
    }
  }
}

export function getScrollOffset(scrollOffset = 240) {
  let offset = 0
  let padding = 24

  if (typeof scrollOffset === 'object' && 'padding' in scrollOffset) {
    padding = scrollOffset.padding
    scrollOffset = scrollOffset.selector
  }

  if (typeof scrollOffset === 'number') {
    offset = scrollOffset
  } else if (typeof scrollOffset === 'string') {
    offset = tryOffsetSelector(scrollOffset, padding)
  } else if (Array.isArray(scrollOffset)) {
    for (const selector of scrollOffset) {
      const res = tryOffsetSelector(selector, padding)
      if (res) {
        offset = res
        break
      }
    }
  }

  return offset
}

function tryOffsetSelector(selector, padding) {
  const el = document.querySelector(selector)
  if (!el) return 0
  const bot = el.getBoundingClientRect().bottom
  if (bot < 0) return 0
  return bot + padding
}


