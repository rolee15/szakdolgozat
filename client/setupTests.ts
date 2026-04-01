import '@testing-library/jest-dom/vitest'

// Optionally, configure React Testing Library defaults here
// e.g., cleanup is automatic with RTL + Vitest when using globals

// jsdom does not implement IntersectionObserver; provide a no-op stub so
// components that use it (e.g. KanjiListPage infinite scroll) do not throw.
const mockIntersectionObserver = vi.fn(() => ({
  observe: vi.fn(),
  unobserve: vi.fn(),
  disconnect: vi.fn(),
}))
vi.stubGlobal('IntersectionObserver', mockIntersectionObserver)
