import { reflow, nextFrame, executeAfterTransition } from '../modules/Utils.js'

const transitionMap = new Map()

/**
 * Handles the transition when an element enters.
 * @param {string} targetId - The ID of the target element.
 * @param {string} classPrefix - Optional prefix for the transition classes.
 * @param {Object} dotnetInstance - The .NET object for invoking methods.
 * @param {string} callbackMethod - The .NET method name to invoke after the transition.
 */
export async function enter(targetId, classPrefix, dotnetInstance, callbackMethod) {
  const element = document.getElementById(targetId)
  if (!element) {
    console.error("Error:", `Element with ID "${targetId}" not found`)
    return
  }

  let transitionData = transitionMap.get(targetId)
  if (!transitionData) {
    const prefix = classPrefix ?? "v"
    transitionData = {
      element,
      enterTransition: {
        from: `${prefix}-enter-from`,
        active: `${prefix}-enter-active`,
        to: `${prefix}-enter-to`
      },
      leaveTransition: {
        from: `${prefix}-leave-from`,
        active: `${prefix}-leave-active`,
        to: `${prefix}-leave-to`
      },
      dotnetInstance,
      callbackMethod
    }
    transitionMap.set(targetId, transitionData)
  }

  applyTransition(element, transitionData.enterTransition, "Enter", dotnetInstance, callbackMethod)
}

/**
 * Handles the transition when an element leaves.
 * @param {string} targetId - The ID of the target element.
 */
export function leave(targetId) {
  if (!targetId) {
    console.error("Error:", "Target ID was not provided.")
    return
  }

  const element = document.getElementById(targetId)
  if (!element) {
    console.error("Error:", `Element with ID "${targetId}" not found`)
    return
  }

  const transitionData = transitionMap.get(targetId)
  if (!transitionData) {
    console.error("Error:", `No transition data found for element "${targetId}"`)
    return
  }

  applyTransition(element, transitionData.leaveTransition, "Leave", transitionData.dotnetInstance, transitionData.callbackMethod)
}

/**
 * Cleans up transition data for an element and removes it from the transition map.
 * @param {string} targetId - The ID of the target element.
 */
export function dispose(targetId) {
  if (transitionMap.has(targetId)) {
    transitionMap.delete(targetId)
  } else {
    console.warn("Warning:", `Element "${targetId}" was not found in transition data`)
  }
}

/**
 * Applies a transition to an element and invokes the corresponding .NET method.
 * @param {HTMLElement} element - The element to apply the transition to.
 * @param {Object} transitionClasses - The transition class names.
 * @param {string} transitionType - The transition type ("Enter" or "Leave").
 * @param {Object} dotnetInstance - The .NET object for invoking methods.
 * @param {string} callbackMethod - The .NET method name.
 */
function applyTransition(element, transitionClasses, transitionType, dotnetInstance, callbackMethod) {
  if (element.classList.contains(transitionClasses.from)) {
    element.classList.remove(transitionClasses.from)
  }
  if (element.classList.contains(transitionClasses.active)) {
    element.classList.remove(transitionClasses.active)
  }
  element.classList.add(transitionClasses.from)
  element.classList.add(transitionClasses.active)

  reflow(element)

  nextFrame(() => {
    element.classList.remove(transitionClasses.from)
    element.classList.add(transitionClasses.to)
  })

  const onTransitionEnd = () => {
    element.classList.remove(transitionClasses.active, transitionClasses.to)
    dotnetInstance.invokeMethodAsync(callbackMethod, `After${transitionType}`)
  }

  executeAfterTransition(onTransitionEnd, element)
}
