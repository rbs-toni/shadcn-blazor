const observers = {
  x: null,
  y: null
}

const disposers = {
  x: null,
  y: null
}

function logInfo(axis, ...args) {
  console.info(`[ScrollbarObserver][${axis.toUpperCase()}]`, ...args)
}

function logWarn(axis, ...args) {
  console.warn(`[ScrollbarObserver][${axis.toUpperCase()}]`, ...args)
}

function logError(axis, ...args) {
  console.error(`[ScrollbarObserver][${axis.toUpperCase()}]`, ...args)
}

function logDebug(axis, ...args) {
  console.debug(`[ScrollbarObserver][${axis.toUpperCase()}]`, ...args)
}

function createObserver(axis, element, dotNetHelper, method, dimension) {
  if (disposers[axis])
    disposers[axis]()

  if (!(element instanceof HTMLElement)) {
    logWarn(axis, 'Invalid element passed (not an HTMLElement)')
    return
  }

  if (!document.body.contains(element)) {
    logWarn(axis, 'Element is not attached to the DOM')
    return
  }

  const observer = new ResizeObserver(entries => {
    try {
      for (const entry of entries) {
        const value = entry.target[dimension]
        if (value > 0) {
          logInfo(axis, `Observed ${dimension}: ${value}, invoking ${method}...`)
          dotNetHelper.invokeMethodAsync(method, value)
            .catch(err => {
              const message = err?.message ?? ''
              if (!/Object reference not set/.test(message))
                logError(axis, `DotNet invocation failed:`, err)
              else
                logDebug(axis, 'DotNet invocation benign failure:', err)
            })
        } else {
          logDebug(axis, `Ignored ${dimension} value <= 0:`, value)
        }
      }
    } catch (err) {
      logError(axis, 'Unhandled error during ResizeObserver callback:', err)
    }
  })

  observer.observe(element)
  observers[axis] = observer

  disposers[axis] = () => {
    try {
      observer.disconnect()
      logInfo(axis, 'Observer disconnected and disposed')
    } catch (e) {
      logError(axis, 'Error during observer cleanup:', e)
    } finally {
      observers[axis] = null
      disposers[axis] = null
    }
  }

  logInfo(axis, 'Observer initialized and observing', element)
}

export function observeScrollbarX(element, dotNetHelper, method) {
  createObserver('x', element, dotNetHelper, method, 'offsetHeight')
}

export function observeScrollbarY(element, dotNetHelper, method) {
  createObserver('y', element, dotNetHelper, method, 'offsetWidth')
}

export function dispose() {
  try {
    disposers.x?.()
    disposers.y?.()
  } catch (e) {
    console.error('[ScrollbarObserver][Global]', 'Error during global disposal:', e)
  } finally {
    observers.x = null
    observers.y = null
    disposers.x = null
    disposers.y = null
    console.info('[ScrollbarObserver][Global]', 'All observers and disposers cleared')
  }
}
