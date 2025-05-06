// Add Stylesheets
hljs_addStylesheet('./_content/ShadcnBlazor.Docs/js/highlightjs/stackoverflow-light.min.css', 'highlight-light', null)
hljs_addStylesheet('./_content/ShadcnBlazor.Docs/js/highlightjs/stackoverflow-dark.min.css', 'highlight-dark', true)
//hljs_addStylesheet('./_content/ShadcnBlazor.Docs/js/highlightjs/hljs-ln-numbers.css', 'hljs-ln-numbers', null)

// Add Scripts
const highlight = hljs_addJavaScript('./_content/ShadcnBlazor.Docs/js/highlightjs/highlight.min.js')

// Add custom code
highlight.onload = () => {
  const hljsRazor = hljs_addJavaScript('./_content/ShadcnBlazor.Docs/js/highlightjs/cshtml-razor.js')
  //const hljslineNumbers = hljs_addJavaScript('./_content/ShadcnBlazor.Docs/js/highlightjs/highlightjs-line-numbers.min.js')

  hljsRazor.onload = () => {
    hljs_ColorSystem()
  }
}

function hljs_ColorSystem() {
  const html = document.documentElement
  let theme

  const observer = new MutationObserver((mutations) => {
    for (const mutation of mutations) {
      if (mutation.type === 'attributes' && mutation.attributeName === 'data-bp-theme') {
        theme = html.getAttribute('data-bp-theme')
        html.setAttribute('data-bs-theme', theme)
        if (theme != null) {
          if (theme == 'null' || theme == null || theme.value === undefined) {
            const isSystemDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches
            hljs_ColorSwitcher(isSystemDark)
          }
        }
        else {
          const isDark = theme === 'dark'
          hljs_ColorSwitcher(isDark)
        }
      }
    }
  })

  observer.observe(html, {
    attributes: true,
    attributeFilter: ['data-bp-theme']
  })
}

function hljs_ColorSwitcher(isDark) {
  const darkCss = document.querySelector('link[title="highlight-dark"]')
  const lightCss = document.querySelector('link[title="highlight-light"]')

  if (isDark) {
    darkCss.removeAttribute("disabled")
    lightCss.setAttribute("disabled", "disabled")
  }
  else {
    lightCss.removeAttribute("disabled")
    darkCss.setAttribute("disabled", "disabled")
  }
}

// Add a <script> to the <body> element
function hljs_addJavaScript(src) {
  const script = document.createElement('script')
  script.type = 'text/javascript'
  script.src = src
  script.async = true

  script.onerror = () => {
    // Error occurred while loading script
    console.error('Error occurred while loading script', src)
  }

  document.body.appendChild(script)

  return script
}

// Add a <link> to the <head> element
function hljs_addStylesheet(src, title, disabled) {
  const stylesheet = document.createElement('link')
  stylesheet.rel = 'stylesheet'
  stylesheet.href = src
  if (title) stylesheet.title = title
  if (disabled) stylesheet.disabled = disabled

  stylesheet.onerror = () => {
    // Error occurred while loading stylesheet
    console.error('Error occurred while loading stylesheet', src)
  }

  document.head.appendChild(stylesheet)

  return stylesheet
}