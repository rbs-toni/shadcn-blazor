export function init(element, ratio = 1) {
    if (!element) return

    // Store original values
    const originalDisplay = element.style.display
    const originalVerticalAlign = element.style.verticalAlign

    // Set required inline styles (matches original)
    element.style.display = 'inline-block'
    element.style.verticalAlign = 'top'

    const parent = element.parentElement
    if (!parent) return

    const setMaxWidth = (width) => {
        element.style.maxWidth = `${width}px`
    }

    // Reset to measure natural width
    element.style.maxWidth = ''
    const parentWidth = parent.clientWidth
    const initialHeight = parent.clientHeight

    // Binary search algorithm (matches original)
    let low = parentWidth / 2 - 0.25
    let high = parentWidth + 0.5

    if (parentWidth) {
        while (low + 1 < high) {
            const mid = Math.round((low + high) / 2)
            setMaxWidth(mid)
            parent.clientHeight === initialHeight ? (high = mid) : (low = mid)
        }
        // Final width calculation with ratio (matches original)
        setMaxWidth(high * ratio + parentWidth * (1 - ratio))
    }

    // Initialize ResizeObserver with cleanup
    if (typeof ResizeObserver !== 'undefined' && !element._wrapObserver) {
        element._wrapObserver = new ResizeObserver(() => {
            init(element, ratio)
        })
        element._wrapObserver.observe(parent)
    }
}

export const dispose = () => {
    if (element._wrapObserver) {
        element._wrapObserver.disconnect()
        delete element._wrapObserver
    }
    element.style.maxWidth = ''
    element.style.display = originalDisplay
    element.style.verticalAlign = originalVerticalAlign
}