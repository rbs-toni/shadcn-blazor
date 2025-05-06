export function getBoundingClientRect(element) {
    if (element && typeof element.getBoundingClientRect === 'function') {
        const rect = element.getBoundingClientRect()
        return {
            top: rect.top,
            left: rect.left,
            width: rect.width,
            height: rect.height
        }
    }

    return {
        top: 0,
        left: 0,
        width: 0,
        height: 0
    }
}