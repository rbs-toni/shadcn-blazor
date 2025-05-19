export function highlightCode(el) {
  if (!el) {
    console.warn('Element not found for code highlighting.')
    return
  }

  try {
    if (hljs) {
      function tryHighlight() {
        if (el.innerText.trim() === '') {
          setTimeout(tryHighlight, 100)
        } else {
          hljs.highlightElement(el)
        }
      }

      tryHighlight()
    } else {
      console.warn('Highlight.js is not loaded.')
    }
  } catch (error) {
    console.error('Error highlighting code:', error)
  }
}