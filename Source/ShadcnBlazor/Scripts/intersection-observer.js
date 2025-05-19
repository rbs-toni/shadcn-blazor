const OBSERVER_ID_PREFIX = "blazor_plugin_observer__";

let OBSERVER_ID = 1;

const observerItems = new Map();

/**
 * Reset the counter and the observer instances
 */
export function reset() {
  OBSERVER_ID = 0;
  observerItems.clear();
}

/**
 * Get the observer items
 */
export function getObserverItems() {
  return observerItems;
}

/**
 * Generate a unique id for an observer item.
 **/
function getObserverElementId() {
  return `${OBSERVER_ID_PREFIX}${OBSERVER_ID++}`;
}

/**
 * Create a new intersection observer item.
 * @param {object} dotnetRef - The current dotnet blazor reference
 * @param {string} callbackId - The callback id for the blazor observer service
 * @param {object} options - The intersection options
 * @returns {object} - The intersection observer item
 */
function createObserverItem(dotnetRef, callbackId, options) {
  const onEntry = onEntryChange(callbackId);

  const observer = new IntersectionObserver(onEntry, options);

  observerItems.set(callbackId, { dotnetRef, observer, elements: [] });

  return observerItems.get(callbackId);
}

/**
 * Observe an element for the observer item
 * @param {string} callbackId - The callback id for the blazor observer service
 * @param {Element} element - The element to observe
 * @returns {string} - The observer element id
 */
export function observeElement(callbackId, element) {
  const item = observerItems.get(callbackId);

  if (item == null) {
    throw new Error(`Failed to observe element for key: ${callbackId} as the observer does not exist`);
  }

  if (item.elements.some(record => record.element == element)) {
    console.warn(`BlazorIntersectionObserver: The element is already being observed by observer for key ${callbackId}`);
    return "";
  }

  const elementId = getObserverElementId();

  item.observer.observe(element);
  item.elements.push({ elementId, element });

  return elementId;
}

/**
 * Create a intersection observer.
 * @param {object} dotnetRef - The dotnet interop reference
 * @param {string} callbackId - The callback id for the blazor observer service
 * @param {object} options - The intersection observer options
 * @returns {object} - The observer item
 */
export function create(dotnetRef, callbackId, options) {
  return createObserverItem(dotnetRef, callbackId, options);
}

/**
 * Observe the target node using a new observer
 * @param {object} dotnetRef - The dotnet interop reference
 * @param {string} callbackId - The callback id for the blazor observer service
 * @param {Element} node - The node to observe
 * @param {object} options - The intersection observer options
 * @returns {string} - The observer element id
 */
export function observe(dotnetRef, callbackId, node, options) {
  createObserverItem(dotnetRef, callbackId, options);
  return observeElement(callbackId, node);
}

/**
 * Unobserve the element for the observer item.
 * @param {string} callbackId - The observer item id
 * @param {Element} element - The element to unobserve
 * @returns {string} - The observer element id that was unobserved
 */
export function unobserve(callbackId, element) {
  const item = observerItems.get(callbackId);

  if (item == null) {
    throw new Error(`Failed to unobserve element for key: ${callbackId} as the observer does not exist`);
  }

  const unobserveElementId = item.elements.find((record) => record.element == element)?.elementId;

  if (unobserveElementId == null) {
    console.warn(`BlazorIntersectionObserver: The record does not exist for observer: ${callbackId}`);
  }

  item.observer.unobserve(element);
  item.elements = item.elements.filter((record) => record.element != element);

  return unobserveElementId;
}

/**
 * Disconnect the observered elements from the observer item.
 * @param {string} callbackId - The observer item id
 * @returns {boolean} - Whether the elements have been removed from the observer item
 */
export function disconnect(callbackId) {
  const item = observerItems.get(callbackId);

  if (item == null) {
    throw new Error(`Failed to disconnect for key: ${callbackId} as the observer does not exist`);
  }

  item.observer.disconnect();
  item.elements = [];

  return true;
}

/**
 * Remove the observer item.
 * @param {string} callbackId - The observer item id
 * @returns {boolean} - Whether the observer item has been removed.
 */
export function remove(callbackId) {
  if (disconnect(callbackId)) {
    return observerItems.delete(callbackId);
  }
  return false;
}

/**
 * Convert the observer entry to an object that will be parsed to the callback.
 * @param {IntersectionObserverEntry} entry - The observer entry
 */
function toEntryObject(entry) {
  function toRectReadOnlyObject(obj) {
    if (!obj) return null;
    return {
      X: obj.x,
      Y: obj.y,
      Width: obj.width,
      Height: obj.height,
      Top: obj.top,
      Left: obj.left,
      Bottom: obj.bottom,
      Right: obj.right,
    };
  }

  return {
    IsIntersecting: entry.isIntersecting,
    IntersectionRatio: entry.intersectionRatio,
    Time: entry.time,
    BoundingClientRect: toRectReadOnlyObject(entry.boundingClientRect),
    IntersectionRect: toRectReadOnlyObject(entry.intersectionRect),
    RootBounds: toRectReadOnlyObject(entry.rootBounds),
  };
}

/**
 * Returns a function that will be triggered when an element has an intersection update.
 * @param {string} callbackId - The observer item id
 * @returns {Function} - The function triggered by an intersection update
 */
function onEntryChange(callbackId) {
  return (entries) => {
    if (!observerItems.has(callbackId)) {
      return;
    }

    const { dotnetRef } = observerItems.get(callbackId);

    const mapped = entries.map((entry) => {
      const mappedEntry = toEntryObject(entry);
      return mappedEntry;
    });

    dotnetRef.invokeMethodAsync(
      "OnCallback",
      callbackId,
      mapped
    );
  };
}