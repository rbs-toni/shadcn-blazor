const THEME_KEY = 'ShadcnBlazorTheme'
const CSS_DISABLE_TRANS = '*,*::before,*::after{-webkit-transition:none!important;-moz-transition:none!important;-o-transition:none!important;-ms-transition:none!important;transition:none!important}'

export function init() {
    const localTheme = localStorage.getItem(THEME_KEY)

    if (localTheme) {
        setTheme(localTheme)
    } else {
        const prefersDarkScheme = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches
        const theme = prefersDarkScheme ? 'dark' : 'light'
        setTheme(theme)
    }
}

export function toggleTheme() {
    const currentTheme = localStorage.getItem(THEME_KEY)
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark'
    localStorage.setItem(THEME_KEY, newTheme)

    setTheme(newTheme)
}

function setTheme(theme) {
    const html = document.documentElement
    const style = document.createElement('style')
    style.appendChild(document.createTextNode(CSS_DISABLE_TRANS))
    document.head.appendChild(style)
    if (theme === 'dark') {
        html.classList.add('dark')
        html.classList.remove('light')
    }
    else {
        html.classList.add('light')
        html.classList.remove('dark')
    }
    const _ = window.getComputedStyle(style).opacity
    document.head.removeChild(style)
}