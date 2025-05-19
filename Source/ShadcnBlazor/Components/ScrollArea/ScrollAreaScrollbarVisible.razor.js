export function getElementInfo(element) {
  if (element) {
    return {
      scrollWidth: element.scrollWidth,
      scrollHeight: element.scrollHeight,
      clientWidth: element.clientWidth,
      clientHeight: element.clientHeight,
      scrollLeft: element.scrollLeft,
      scrollTop: element.scrollTop
    }
  }
}

export function setElementInfo(element, prop, value) {
  if (element) {
    const parts = prop.split('.');
    let target = element;

    for (let i = 0; i < parts.length - 1; i++) {
      target = target[parts[i]];
    }

    target[parts[parts.length - 1]] = value;
  }
}