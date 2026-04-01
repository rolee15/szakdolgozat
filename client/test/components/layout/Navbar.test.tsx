import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import Navbar from '@/components/layout/Navbar'

vi.mock('@/components/layout/Logo', () => ({ default: () => <div data-testid="logo">Logo</div> }))

const mockUseAuth = vi.fn()
vi.mock('@/context/AuthContext', () => ({
  useAuth: () => mockUseAuth(),
}))

describe('Navbar', () => {
  it('renders Logo, menu items, and login/register links when not authenticated', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: false, username: null, logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    expect(screen.getByTestId('logo')).toBeInTheDocument()

    expect(screen.getByRole('link', { name: 'Hiragana' })).toHaveAttribute('href', '/hiragana')
    expect(screen.getByRole('link', { name: 'Katakana' })).toHaveAttribute('href', '/katakana')
    expect(screen.getByRole('link', { name: 'Lessons' })).toHaveAttribute('href', '/lessons')

    expect(screen.getByRole('link', { name: /login/i })).toHaveAttribute('href', '/login')
    expect(screen.getByRole('link', { name: /register/i })).toHaveAttribute('href', '/register')
  })

  it('shows username and logout button when authenticated', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: true, username: 'testuser', logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    expect(screen.getByText('testuser')).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /logout/i })).toBeInTheDocument()
  })

  it('shows Admin link when user is admin', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: true, username: 'adminuser', isAdmin: true, logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    const adminLink = screen.getByRole('link', { name: /^admin$/i })
    expect(adminLink).toBeInTheDocument()
    expect(adminLink).toHaveAttribute('href', '/admin')
  })

  it('hides Admin link when user is not admin', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: true, username: 'regularuser', isAdmin: false, logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    expect(screen.queryByRole('link', { name: /^admin$/i })).not.toBeInTheDocument()
  })
})
