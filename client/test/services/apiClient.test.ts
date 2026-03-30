import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'

// apiFetch reads localStorage and calls fetch at call time, so we can import the module
// after setting up localStorage/fetch mocks in each test.
import { apiFetch } from '@/services/apiClient'

function mockFetchWithStatus(status: number, body: unknown = {}) {
  const json = vi.fn().mockResolvedValue(body)
  const response = { ok: status >= 200 && status < 300, status, json } as unknown as Response
  ;(globalThis as { fetch: typeof fetch }).fetch = vi
    .fn()
    .mockResolvedValue(response) as unknown as typeof fetch
  return response
}

describe('apiClient', () => {
  beforeEach(() => {
    localStorage.clear()
    vi.restoreAllMocks()
    // Reset window.location.href to a safe value before each test
    Object.defineProperty(window, 'location', {
      value: { href: 'http://localhost/' },
      writable: true,
    })
  })

  afterEach(() => {
    localStorage.clear()
    vi.clearAllMocks()
  })

  it('apiFetch_addsAuthorizationHeader_whenTokenInLocalStorage', async () => {
    localStorage.setItem('token', 'test-jwt-token')
    mockFetchWithStatus(200)

    await apiFetch('http://api.test/resource')

    const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>
    const [, init] = fetchMock.mock.calls[0] as [string, RequestInit]
    const headers = init.headers as Record<string, string>
    expect(headers['Authorization']).toBe('Bearer test-jwt-token')
  })

  it('apiFetch_noHeader_whenNoToken', async () => {
    mockFetchWithStatus(200)

    await apiFetch('http://api.test/resource')

    const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>
    const [, init] = fetchMock.mock.calls[0] as [string, RequestInit]
    const headers = init.headers as Record<string, string>
    expect(headers['Authorization']).toBeUndefined()
  })

  it('apiFetch_setsContentTypeHeader', async () => {
    mockFetchWithStatus(200)

    await apiFetch('http://api.test/resource')

    const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>
    const [, init] = fetchMock.mock.calls[0] as [string, RequestInit]
    const headers = init.headers as Record<string, string>
    expect(headers['Content-Type']).toBe('application/json')
  })

  it('apiFetch_redirectsToLogin_on401', async () => {
    mockFetchWithStatus(401)

    await apiFetch('http://api.test/resource')

    expect(window.location.href).toBe('/login')
    expect(localStorage.getItem('token')).toBeNull()
    expect(localStorage.getItem('refreshToken')).toBeNull()
  })

  it('apiFetch_removesTokensFromLocalStorage_on401', async () => {
    localStorage.setItem('token', 'expiredtoken')
    localStorage.setItem('refreshToken', 'expiredrefresh')
    mockFetchWithStatus(401)

    await apiFetch('http://api.test/resource')

    expect(localStorage.getItem('token')).toBeNull()
    expect(localStorage.getItem('refreshToken')).toBeNull()
  })

  it('apiFetch_returnsResponse_onSuccess', async () => {
    mockFetchWithStatus(200, { data: 'ok' })

    const response = await apiFetch('http://api.test/resource')

    expect(response.status).toBe(200)
    expect(response.ok).toBe(true)
  })

  it('apiFetch_forwardsRequestOptionsExceptHeaders', async () => {
    mockFetchWithStatus(200)

    await apiFetch('http://api.test/resource', {
      method: 'POST',
      body: JSON.stringify({ key: 'value' }),
    })

    const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>
    const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit]
    expect(url).toBe('http://api.test/resource')
    expect(init.method).toBe('POST')
    expect(init.body).toBe(JSON.stringify({ key: 'value' }))
  })

  it('apiFetch_doesNotRedirect_on200', async () => {
    const originalHref = window.location.href
    mockFetchWithStatus(200)

    await apiFetch('http://api.test/resource')

    expect(window.location.href).toBe(originalHref)
  })
})
