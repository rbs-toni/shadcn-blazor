export function getDocumentAttribute(attributeName) {
  if (typeof attributeName !== 'string' || attributeName.trim() === '') {
    throw new Error('attributeName must be a non-empty string')
  }

  return document.documentElement.getAttribute(attributeName)
}