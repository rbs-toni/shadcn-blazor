const ICON_URL = "./_content/ShadcnBlazor/files/BootstrapIconMetadata.json"
const CACHE_KEY = "ShadcnBlazor.Docs"
const CACHE_EXPIRATION_KEY = "ShadcnBlazor.Docs.Expiration"
const CACHE_TTL = 24 * 60 * 60 * 1000
const MAX_RETRIES = 3
const BASE_DELAY = 500

async function fetchWithRetry(url, retries = MAX_RETRIES, delay = BASE_DELAY) {
  for (let attempt = 0; attempt <= retries; attempt++) {
    try {
      const response = await fetch(url)
      if (response.ok) {
        return response.json()
      }
      console.warn(`Fetch failed: ${response.status} ${response.statusText}`);
    } catch (error) {
      console.error("Fetch error:", error)
    }

    if (attempt < retries) {
      const waitTime = delay * 2 ** attempt
      await new Promise(resolve => setTimeout(resolve, waitTime))
    }
  }
  console.error(`Fetch failed after ${retries + 1} attempts`)
  return null;
}

async function fetchAndCacheData() {
  const data = await fetchWithRetry(ICON_URL)
  if (!data) {
    console.warn("No data fetched. Cache update skipped.")
    return null;
  }

  localStorage.setItem(CACHE_KEY, JSON.stringify(data))
  localStorage.setItem(CACHE_EXPIRATION_KEY, (Date.now() + CACHE_TTL).toString())
  return data;
}

async function getData() {
  const expiration = parseInt(localStorage.getItem(CACHE_EXPIRATION_KEY) || "0", 10)
  if (expiration > Date.now()) {
    const cachedData = localStorage.getItem(CACHE_KEY)
    return cachedData ? JSON.parse(cachedData) : null
  }
  return fetchAndCacheData();
}

export async function getCategoryStats() {
  const metadata = await getData();
  if (!metadata || !Array.isArray(metadata)) {
    console.warn("No metadata available");
    return [];
  }

  // Count categories
  const rawCounts = new Map();
  for (const { categories } of metadata) {
    if (!Array.isArray(categories)) continue;
    for (const category of categories) {
      rawCounts.set(category, (rawCounts.get(category) || 0) + 1)
    }
  }

  // Merge counts with normalized categories
  const mergedCounts = new Map();
  for (const [rawCategory, count] of rawCounts.entries()) {
    const normalized = rawCategory.trim()
      .toLowerCase()
      .split(/\s+/)
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(" ");
    mergedCounts.set(normalized, (mergedCounts.get(normalized) || 0) + count)
  }

  // Sort and format results
  return [...mergedCounts.entries()]
    .sort(([a], [b]) => a.localeCompare(b))
    .map(([category, count]) => ({ category, count }));
}

export async function search(keyword, category, page = 1, pageSize = -1, sortOptions = { key: "title", order: "asc" }) {
  const metadata = await getData()
  if (!metadata || !Array.isArray(metadata)) {
    console.warn("No metadata found")
    return {
      page,
      pageSize,
      totalItems: 0,
      totalPages: 0,
      hasNextPage: false,
      hasPrevPage: false,
      items: []
    };
  }

  let filtered = metadata
  const lowerKeyword = keyword?.toLowerCase()
  const lowerCategory = category?.toLowerCase()

  if (lowerKeyword) {
    filtered = metadata.filter(({ title, categories, tags }) =>
      title.toLowerCase().includes(lowerKeyword) ||
      categories?.some(cat => cat.toLowerCase().includes(lowerKeyword)) ||
      tags?.some(tag => tag.toLowerCase().includes(lowerKeyword))
    );
  }

  if (lowerCategory) {
    filtered = filtered.filter(({ categories }) =>
      categories?.some(cat => cat.toLowerCase() === lowerCategory)
    )
  }

  if (sortOptions?.key && filtered.length > 0 && sortOptions.key in filtered[0]) {
    filtered.sort((a, b) => {
      const aValue = a[sortOptions.key] || ""
      const bValue = b[sortOptions.key] || ""
      return aValue.localeCompare(bValue) * (sortOptions.order === "desc" ? -1 : 1)
    });
  }

  const totalItems = filtered.length
  const totalPages = pageSize > 0 ? Math.ceil(totalItems / pageSize) : 0

  return {
    grandTotal: metadata.length,
    page,
    pageSize,
    totalItems,
    totalPages,
    hasNextPage: pageSize > 0 && page < totalPages,
    hasPrevPage: page > 1 && pageSize > 0,
    items: pageSize === -1 ? filtered : filtered.slice((page - 1) * pageSize, page * pageSize)
  }
}