// DOM Measurement
export function getBoundingClientRect(element) {
  const rect = element.getBoundingClientRect()
  return {
    x: rect.x,
    y: rect.y,
    width: rect.width,
    height: rect.height,
    top: rect.top,
    right: rect.right,
    bottom: rect.bottom,
    left: rect.left
  }
}

export function getElementInfo(element) {
  return {
    clientWidth: element.clientWidth,
    clientHeight: element.clientHeight,
  }
}

export function getOffsetTop(element) {
  return element.offsetTop
}

export function getOffsetLeft(element) {
  return element.offsetLeft
}

export function getScrollTop(element) {
  return element.scrollTop
}

export function setScrollTop(element, value) {
  element.scrollTop = value
}

// Visibility
export function isElementVisible(element) {
  const style = window.getComputedStyle(element)
  return style.display !== 'none' && style.visibility !== 'hidden' && style.opacity !== '0'
}

export function isElementInViewport(element) {
  const rect = element.getBoundingClientRect()
  return (
    rect.top >= 0 &&
    rect.left >= 0 &&
    rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
    rect.right <= (window.innerWidth || document.documentElement.clientWidth)
  )
}

export function scrollIntoView(element, smooth = false) {
  element.scrollIntoView({
    behavior: smooth ? 'smooth' : 'auto',
    block: 'nearest'
  })
}

// Focus Management
export function focus(element) {
  element.focus()
}

export function blur(element) {
  element.blur()
}

export function hasFocus(element) {
  return document.activeElement === element
}

// CSS Classes
export function addClass(element, className) {
  element.classList.add(className)
}

export function removeClass(element, className) {
  element.classList.remove(className)
}

export function toggleClass(element, className) {
  element.classList.toggle(className)
}

export function containsClass(element, className) {
  return element.classList.contains(className)
}

// Attributes
export function setAttribute(element, name, value) {
  element.setAttribute(name, value)
}

export function getAttribute(element, name) {
  return element.getAttribute(name)
}

export function removeAttribute(element, name) {
  element.removeAttribute(name)
}

// Styles
export function setStyle(element, property, value) {
  element.style[property] = value
}

export function getStyle(element, property) {
  return window.getComputedStyle(element)[property]
}

// Events
export function addEventListener(element, eventName, dotNetHelper, options) {
  const handler = (e) => {
    dotNetHelper.invokeMethodAsync('Invoke', e)
  }
  element.addEventListener(eventName, handler, options)
  return { eventName, handler }
}

export function removeEventListener(element, eventName, dotNetHelper) {
  // Note: You'll need to store handlers somewhere to properly remove them
  // This is a simplified version
  const handler = (e) => {
    dotNetHelper.invokeMethodAsync('Invoke', e)
  }
  element.removeEventListener(eventName, handler)
}

// Text/HTML
export function getTextContent(element) {
  return element.textContent
}

export function setTextContent(element, content) {
  element.textContent = content
}

export function getInnerHtml(element) {
  return element.innerHTML
}

export function setInnerHtml(element, html) {
  element.innerHTML = html
}

// RTL
export function isRtl(element) {
  return window.getComputedStyle(element).direction === 'rtl'
}

// Window/Viewport
export function getWindowInnerWidth() {
  return window.innerWidth
}

export function getWindowInnerHeight() {
  return window.innerHeight
}

export function getDevicePixelRatio() {
  return window.devicePixelRatio
}

// Clipboard
export async function copyToClipboard(text) {
  await navigator.clipboard.writeText(text)
}

export async function readFromClipboard() {
  return await navigator.clipboard.readText()
}

// Misc
export function click(element) {
  element.click()
}

export function preventDefault(element, eventName) {
  element.addEventListener(eventName, e => e.preventDefault())
}

export function stopPropagation(element, eventName) {
  element.addEventListener(eventName, e => e.stopPropagation())
}