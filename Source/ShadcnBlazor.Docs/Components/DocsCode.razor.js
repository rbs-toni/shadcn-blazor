export function highlightCode(el) {
  if (!el) {
    console.warn('Element not found for code highlighting.')
    return
  }

  try {
    // Check if Highlight.js is available before calling
    if (hljs) {
      // Check if the element has content, and wait until it does
      function tryHighlight() {
        if (el.innerText.trim() === '') {
          setTimeout(tryHighlight, 100) // Check again after a short delay
        } else {
          hljs.highlightElement(el)
        }
      }

      tryHighlight() // Initial call to tryHighlight function
    } else {
      console.warn('Highlight.js is not loaded.')
    }
  } catch (error) {
    // Handle any errors that occur during the highlighting process
    console.error('Error highlighting code:', error)
  }
}