export function getHeaders() {
  const headers = []

  // Collect all headers with an anchor link to the section
  document.querySelectorAll('h2, h3, h4, h5, h6').forEach(header => {
    const anchor = header.querySelector('a[aria-label="Link to section"]')
    if (anchor && header.id) {
      headers.push({
        level: header.tagName[1],  // 'H2' -> '2', 'H3' -> '3', etc.
        title: header.innerText,
        href: `#${header.id}`,
        children: []
      })
    }
  })

  const tree = []  // The final tree of headers
  const stack = [] // Stack to maintain the current path for nesting

  headers.forEach(header => {
    const node = {
      Level: header.level,
      Title: header.title,
      Href: header.href,
      Items: []
    }

    // Pop headers from the stack that are of greater or equal level than the current header
    while (stack.length && parseInt(stack[stack.length - 1].Level) >= parseInt(header.level)) {
      stack.pop()
    }

    // Add the node either as a child or root element based on the stack's current state
    if (stack.length) {
      stack[stack.length - 1].Items.push(node)  // Add as a child
    } else {
      tree.push(node)  // Add as a root
    }

    // Push the current node to the stack for future nesting
    stack.push(node)
  })

  return tree
}
