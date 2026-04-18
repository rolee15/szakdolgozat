import { describe, it, expect, vi } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
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

    expect(screen.getAllByRole('link', { name: 'Hiragana' })[0]).toHaveAttribute('href', '/hiragana')
    expect(screen.getAllByRole('link', { name: 'Katakana' })[0]).toHaveAttribute('href', '/katakana')
    expect(screen.getAllByRole('link', { name: 'Lessons' })[0]).toHaveAttribute('href', '/lessons')

    expect(screen.getAllByRole('link', { name: /login/i })[0]).toHaveAttribute('href', '/login')
    expect(screen.getAllByRole('link', { name: /register/i })[0]).toHaveAttribute('href', '/register')
  })

  it('shows username and logout button when authenticated', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: true, username: 'testuser', logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    // username appears in both desktop and mobile auth areas
    const usernameEls = screen.getAllByText('testuser')
    expect(usernameEls.length).toBeGreaterThan(0)
    // logout button appears in both desktop and mobile areas
    const logoutBtns = screen.getAllByRole('button', { name: /logout/i })
    expect(logoutBtns.length).toBeGreaterThan(0)
  })

  it('shows Admin link when user is admin', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: true, username: 'adminuser', isAdmin: true, logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    const adminLinks = screen.getAllByRole('link', { name: /^admin$/i })
    expect(adminLinks.length).toBeGreaterThan(0)
    expect(adminLinks[0]).toHaveAttribute('href', '/admin')
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

  it('toggles mobile menu open and closed via hamburger button', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: false, username: null, logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    const openButton = screen.getByRole('button', { name: /open menu/i })
    expect(openButton).toBeInTheDocument()

    // Mobile nav group labels should not be visible before opening
    // (they exist in mobile dropdown which is hidden)
    expect(screen.queryByRole('button', { name: /close menu/i })).not.toBeInTheDocument()

    fireEvent.click(openButton)

    expect(screen.getByRole('button', { name: /close menu/i })).toBeInTheDocument()
  })

  it('shows all three group labels Study, Practice, Path', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: false, username: null, logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    // These appear in the desktop nav
    const studyLabels = screen.getAllByText('Study')
    expect(studyLabels.length).toBeGreaterThan(0)

    const practiceLabels = screen.getAllByText('Practice')
    expect(practiceLabels.length).toBeGreaterThan(0)

    const pathLabels = screen.getAllByText('Path')
    expect(pathLabels.length).toBeGreaterThan(0)
  })

  it('closes mobile menu when a link is clicked', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: false, username: null, logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    fireEvent.click(screen.getByRole('button', { name: /open menu/i }))
    expect(screen.getByRole('button', { name: /close menu/i })).toBeInTheDocument()

    // Click a link in the mobile dropdown
    const hiraganaLinks = screen.getAllByRole('link', { name: 'Hiragana' })
    // Find the one inside the mobile nav (second occurrence)
    fireEvent.click(hiraganaLinks[hiraganaLinks.length - 1])

    expect(screen.queryByRole('button', { name: /close menu/i })).not.toBeInTheDocument()
  })

  it('shows logout button in mobile auth area when authenticated', () => {
    mockUseAuth.mockReturnValue({ isAuthenticated: true, username: 'mobileuser', isAdmin: false, logout: vi.fn() })

    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    // username appears in both desktop and mobile auth areas
    expect(screen.getAllByText('mobileuser').length).toBeGreaterThan(0)
    // both desktop and mobile logout buttons rendered
    expect(screen.getAllByRole('button', { name: /logout/i }).length).toBeGreaterThan(0)
  })
})
