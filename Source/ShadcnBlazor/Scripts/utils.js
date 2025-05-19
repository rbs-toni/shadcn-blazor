const MILLISECONDS = 1000
const TRANSITION_END = 'transitionend'

/**
 * Dispatches a synthetic transitionend event.
 */
const triggerTransitionEnd = element =>
  element.dispatchEvent(new Event(TRANSITION_END))

/**
 * Executes a function if it's callable, otherwise returns a fallback value.
 */
const execute = (fn, args = [], fallback = fn) =>
  typeof fn === 'function' ? fn(...args) : fallback

/**
 * Forces a reflow, triggering browser layout calculations.
 */
const reflow = element => element.offsetHeight

/**
 * Returns the total transition time (duration + delay) in milliseconds.
 */
const getTransitionDurationFromElement = element => {
  if (!element) return 0

  const { transitionDuration, transitionDelay } = window.getComputedStyle(element)

  const parseTimeList = str =>
    str.split(',').map(time => parseFloat(time.trim()) || 0)

  const duration = parseTimeList(transitionDuration)[0]
  const delay = parseTimeList(transitionDelay)[0]

  return (duration + delay) * MILLISECONDS
}

let tickQueue = []
let isHoldingTicks = false

/**
 * Queues a callback to run in the next microtask tick.
 */
const nextTick = (callback = () => { }) =>
  new Promise(resolve => {
    queueMicrotask(() => {
      if (!isHoldingTicks) {
        setTimeout(releaseNextTicks)
      }
    })

    tickQueue.push(() => {
      callback()
      resolve()
    })
  })

/**
 * Releases all queued nextTick callbacks.
 */
const releaseNextTicks = () => {
  isHoldingTicks = false
  while (tickQueue.length) tickQueue.shift()()
}

/**
 * Prevents nextTick callbacks from running until released.
 */
const holdNextTicks = () => {
  isHoldingTicks = true
}

/**
 * Executes a callback after the transition ends (or after a fallback timeout).
 */
const executeAfterTransition = (callback, element, wait = true) => {
  if (!wait) {
    execute(callback)
    return
  }

  const fallbackPadding = 5
  const timeout = getTransitionDurationFromElement(element) + fallbackPadding

  let handled = false

  const handler = event => {
    if (event.target !== element) return

    handled = true
    element.removeEventListener(TRANSITION_END, handler)
    execute(callback)
  }

  element.addEventListener(TRANSITION_END, handler)

  setTimeout(() => {
    if (!handled) {
      triggerTransitionEnd(element)
    }
  }, timeout)
}

export {
  triggerTransitionEnd,
  executeAfterTransition,
  getTransitionDurationFromElement,
  reflow,
  nextTick,
  holdNextTicks,
  releaseNextTicks
}
