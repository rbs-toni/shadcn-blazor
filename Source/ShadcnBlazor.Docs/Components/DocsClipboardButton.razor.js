const clipboardMap = new WeakMap()

export function init(el, target) {
  if (!el || !target) {
    console.warn('init: element or target ID is missing')
    return
  }

  const codeEl = document.getElementById(target)
  if (!codeEl) {
    console.warn(`init: element with ID "${target}" not found`)
    return
  }

  const handler = () => {
    const code = codeEl.innerText
    navigator.clipboard.writeText(code).then(() => {
      el.setAttribute('data-state', 'copied')
      setTimeout(() => {
        el.setAttribute('data-state', 'copy')
      }, 1500)
    }).catch(err => {
      console.warn('init: failed to copy text', err)
    })
  }

  el.addEventListener('click', handler)
  clipboardMap.set(el, handler)
}

export function dispose(el) {
  const handler = clipboardMap.get(el)
  if (handler) {
    el.removeEventListener('click', handler)
    clipboardMap.delete(el)
  }
}