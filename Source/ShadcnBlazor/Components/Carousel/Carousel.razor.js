import { EmblaCarousel } from '../../modules/embla-carousel.min.js'

function handleSelect(embla, dotnet, method) {
  const canScrollNext = embla.canScrollNext() || false
  const canScrollPrev = embla.canScrollPrev() || false

  dotnet.invokeMethodAsync(method, { canScrollNext, canScrollPrev })
}

export const embla = {
  init(element, options, dotnet, method) {
    if (!element) return

    const instance = EmblaCarousel(element, options)

    const onSelect = () => handleSelect(instance, dotnet, method)

    instance.on('init', onSelect)
    instance.on('reInit', onSelect)
    instance.on('select', onSelect)

    return instance
  },

  scrollPrev(instance, jump) {
    if (!instance) return
    jump ? instance.scrollPrev(jump) : instance.scrollPrev()
  },

  scrollNext(instance, jump) {
    if (!instance) return
    jump ? instance.scrollNext(jump) : instance.scrollNext()
  },

  dispose(instance) {
    instance?.destroy()
  }
}
