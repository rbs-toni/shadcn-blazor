let mediaQueryMap = new Map()

export function register(dotnet, method, query) {
  // Ensure the provided arguments are valid
  if (!dotnet || !method || !Array.isArray(query) || query.length === 0) {
    console.warn("Invalid arguments provided")
    return
  }

  query.forEach(mediaQueryString => {
    const mediaQuery = window.matchMedia(mediaQueryString)

    const matchesHandler = (e) => {
      if (e.matches) {
        dotnet.invokeMethodAsync(method, mediaQueryString)
      }
    }

    mediaQueryMap.set(mediaQueryString, { mediaQuery, matchesHandler })
    mediaQuery.addEventListener('change', matchesHandler)
  })
}

export function dispose() {
  mediaQueryMap.forEach(({ mediaQuery, matchesHandler }, mediaQueryString) => {
    mediaQuery.removeEventListener('change', matchesHandler)
  })
  mediaQueryMap.clear()
}
