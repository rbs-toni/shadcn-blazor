const timeoutIdMap = new WeakMap()

export function addCopyButton(el) {
  if (!el) {
    console.warn("Element not provided")
    return
  }

  el.addEventListener('click', (e) => {
    const target = e.target
    if (target.matches('div[class*="language-"] > button.copy')) {
      const parent = target.parentElement
      const sibling = target.nextElementSibling?.nextElementSibling
      if (!parent || !sibling) {
        return
      }

      const isShell = /language-(shellscript|shell|bash|sh|zsh)/.test(parent.className)
      const ignoredNodes = ['.vp-copy-ignore', '.diff.remove']

      // Clone the node and remove the ignored nodes
      const clone = sibling.cloneNode(true)
      ignoredNodes.forEach((selector) => {
        clone.querySelectorAll(selector).forEach((node) => node.remove())
      })

      let text = clone.textContent || ''
      if (isShell) {
        text = text.replace(/^ *(\$|>) /gm, '').trim()
      }

      copyToClipboard(text).then(() => {
        target.classList.add('copied')
        clearTimeout(timeoutIdMap.get(target))
        const timeoutId = setTimeout(() => {
          target.classList.remove('copied')
          target.blur()
          timeoutIdMap.delete(target)
        }, 2000)
        timeoutIdMap.set(target, timeoutId)
      })
    }
  })
}

async function copyToClipboard(text) {
  try {
    await navigator.clipboard.writeText(text)
  } catch {
    const element = document.createElement('textarea')
    const previouslyFocusedElement = document.activeElement

    element.value = text
    element.setAttribute('readonly', '')
    element.style.contain = 'strict'
    element.style.position = 'absolute'
    element.style.left = '-9999px'
    element.style.fontSize = '12pt' // Prevent zooming on iOS

    const selection = document.getSelection()
    const originalRange = selection ? (selection.rangeCount > 0 && selection.getRangeAt(0)) : null

    document.body.appendChild(element)
    element.select()
    element.selectionStart = 0
    element.selectionEnd = text.length

    document.execCommand('copy')
    document.body.removeChild(element)

    if (originalRange) {
      selection.removeAllRanges()
      selection.addRange(originalRange)
    }

    if (previouslyFocusedElement) {
      previouslyFocusedElement.focus()
    }
  }
}

export function highlightElement(el) {
  try {
    hljs?.highlightElement(el)
  } catch {
    // HighlightJS may fail when the user navigates away from the page quickly,
    // which, when not caught, will result in a crash.
  }
}
