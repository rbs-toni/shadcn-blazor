export function init(element) {
  if (element) {
    element.addEventListener('pointerdown', e => {
      if (!element.contains(e.target)) {
        e.preventDefault()
      }
    })
  }
}