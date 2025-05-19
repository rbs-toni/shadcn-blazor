import { reflow, nextTick, executeAfterTransition } from '../../modules/utils.min.js'

const transitionMap = new Map()

export async function enter(targetId, classes, dotnet, callback) {
  const element = getElement(targetId)
  if (!element) return

  applyTransition(element, classes, 'Enter', dotnet, callback)
}

export function leave(targetId, classes, dotnet, callback) {
  const element = getElement(targetId)
  if (!element) return

  applyTransition(element, classes, 'Leave', dotnet, callback)
}

export function dispose(targetId) {
  if (transitionMap.has(targetId)) {
    transitionMap.delete(targetId)
  } else {
    console.warn(`Warning: Element "${targetId}" was not found in transition data`)
  }
}

function getElement(id) {
  if (!id) {
    console.error('Error: Target ID was not provided.')
    return null
  }

  const element = document.getElementById(id)
  if (!element) {
    console.error(`Error: Element with ID "${id}" not found`)
    return null
  }

  return element
}

function applyTransition(element, classes, transitionType, dotnet, callback) {
  const from = toClassList(classes.from)
  const active = toClassList(classes.active)
  const to = toClassList(classes.to)

  element.classList.add(...from)
  reflow(element)
  element.classList.add(...active)
  requestAnimationFrame(() => {
    requestAnimationFrame(() => {
      element.classList.remove(...from)
      element.classList.add(...to)
    })
  })

  const onTransitionEnd = () => {
    element.classList.remove(...active, ...to)
    dotnet.invokeMethodAsync(callback, `After${transitionType}`)
  }

  executeAfterTransition(onTransitionEnd, element)
}

function toClassList(value) {
  if (!value) return []
  return typeof value === 'string' ? value.trim().split(/\s+/) : []
}
