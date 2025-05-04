export function getHeaders() {
    const headers = []

    document.querySelectorAll('h2, h3, h4, h5, h6').forEach(header => {
        const anchor = header.querySelector('a[aria-label="Link to section"]')
        if (anchor && header.id) {
            headers.push({
                level: header.tagName[1],
                title: header.innerText,
                href: `#${header.id}`,
                children: []
            })
        }
    })

    const tree = []
    const stack = []

    headers.forEach(header => {
        const node = {
            Level: header.level,
            Title: header.title,
            Href: header.href,
            Items: []
        }

        while (stack.length && parseInt(stack[stack.length - 1].Level[1]) >= parseInt(header.level[1])) {
            stack.pop()
        }

        stack.length ? stack[stack.length - 1].Items.push(node) : tree.push(node)

        stack.push(node)
    })
    return tree
}