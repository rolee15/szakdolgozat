import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

vi.mock('@/services/userService', () => ({
  default: {
    activateAccount: vi.fn(),
  },
}))

import userService from '@/services/userService'
import ActivatePage from '@/pages/ActivatePage'

const svc = userService as unknown as {
  activateAccount: ReturnType<typeof vi.fn>
}

function renderPage(search = '') {
  const queryClient = new QueryClient({
    defaultOptions: { queries: { retry: false }, mutations: { retry: false } },
  })
  return render(
    <QueryClientProvider client={queryClient}>
      <MemoryRouter initialEntries={[`/activate${search}`]}>
        <ActivatePage />
      </MemoryRouter>
    </QueryClientProvider>
  )
}

describe('ActivatePage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders loading state while activating', async () => {
    svc.activateAccount.mockReturnValue(new Promise(() => {}))

    renderPage('?token=abc123')

    expect(await screen.findByText(/activating your account/i)).toBeInTheDocument()
  })

  it('renders success message on successful activation', async () => {
    svc.activateAccount.mockResolvedValue({
      success: true,
      message: 'Account activated. You can now log in.',
    })

    renderPage('?token=abc123')

    expect(await screen.findByText(/account activated\. you can now log in/i)).toBeInTheDocument()
    expect(screen.getByRole('link', { name: /go to login/i })).toBeInTheDocument()
  })

  it('renders error message on failed activation with Error instance', async () => {
    svc.activateAccount.mockRejectedValue(new Error('Token expired'))

    renderPage('?token=badtoken')

    expect(await screen.findByText(/token expired/i)).toBeInTheDocument()
    expect(screen.getByRole('link', { name: /go to login/i })).toBeInTheDocument()
  })

  it('renders generic error message on failed activation with non-Error', async () => {
    svc.activateAccount.mockRejectedValue('unexpected failure')

    renderPage('?token=badtoken')

    expect(await screen.findByText(/activation failed\. please try again/i)).toBeInTheDocument()
  })

  it('renders fallback message when API response has no message', async () => {
    svc.activateAccount.mockResolvedValue({ success: true, message: null })

    renderPage('?token=abc123')

    expect(await screen.findByText(/account activated\. you can now log in/i)).toBeInTheDocument()
  })

  it('renders error when token is missing from URL', () => {
    renderPage('')

    expect(screen.getByText(/invalid activation link/i)).toBeInTheDocument()
    expect(screen.getByRole('link', { name: /go to login/i })).toBeInTheDocument()
    expect(svc.activateAccount).not.toHaveBeenCalled()
  })
})
