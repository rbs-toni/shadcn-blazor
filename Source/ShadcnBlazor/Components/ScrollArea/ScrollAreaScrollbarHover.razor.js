let handlePointerEnter = null
let handlePointerLeave = null
let internalScrollArea = null

export function init(scrollArea, dotnet) {
  if (scrollArea) {
    internalScrollArea = scrollArea
    handlePointerEnter = () => {
      dotnet.invokeMethodAsync("OnPointerEnter")
    }
    handlePointerLeave = () => {
      dotnet.invokeMethodAsync("OnPointerLeave")
    }
    internalScrollArea.addEventListener('pointerenter', handlePointerEnter)
    internalScrollArea.addEventListener('pointerleave', handlePointerLeave)
  }
}

export function dispose() {
  if (internalScrollArea) {
    internalScrollArea.removeEventListener('pointerenter', handlePointerEnter)
    internalScrollArea.removeEventListener('pointerleave', handlePointerLeave)
  }
}