import { describe, it, expect, vi } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import Navbar from '@/components/layout/Navbar'

vi.mock('@/components/layout/Logo', () => ({ default: () => <div data-testid="logo">Logo</div> }))

const mockNavigate = vi.fn()
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual('react-router-dom')
  return { ...actual, useNavigate: () => mockNavigate }
})

const mockUseAuth = vi.fn()
vi.mock('@/context/AuthContext', () => ({
  useAuth: () => mockUseAuth(),
}))

const defaultAuth = { isAuthenticated: false, username: null, isAdmin: false, logout: vi.fn() }
const authUser = { isAuthenticated: true, username: 'testuser', isAdmin: false, logout: vi.fn() }
const authAdmin = { isAuthenticated: true, username: 'adminuser', isAdmin: true, logout: vi.fn() }

function renderNavbar(auth = defaultAuth) {
  mockUseAuth.mockReturnValue(auth)
  return render(
    <MemoryRouter>
      <Navbar />
    </MemoryRouter>
  )
}

describe('Navbar', () => {
  it('renders the Logo', () => {
    renderNavbar()
    expect(screen.getByTestId('logo')).toBeInTheDocument()
  })

  it('shows only Login and Register when not authenticated, no nav dropdowns', () => {
    renderNavbar()

    const loginLinks = screen.getAllByRole('link', { name: /login/i })
    expect(loginLinks.length).toBeGreaterThan(0)
    expect(loginLinks[0]).toHaveAttribute('href', '/login')

    const registerLinks = screen.getAllByRole('link', { name: /register/i })
    expect(registerLinks.length).toBeGreaterThan(0)
    expect(registerLinks[0]).toHaveAttribute('href', '/register')

    expect(screen.queryByRole('button', { name: /lessons/i })).not.toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /reviews/i })).not.toBeInTheDocument()
  })

  it('shows Lessons dropdown with learning content when authenticated', () => {
    renderNavbar(authUser)

    const lessonsButtons = screen.getAllByRole('button', { name: /lessons/i })
    expect(lessonsButtons.length).toBeGreaterThan(0)

    fireEvent.click(lessonsButtons[0])

    expect(screen.getByRole('link', { name: 'Hiragana' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Katakana' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Kanji' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Grammar' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Reading' })).toBeInTheDocument()
  })

  it('shows Reviews dropdown with practice content when authenticated', () => {
    renderNavbar(authUser)

    const reviewsButtons = screen.getAllByRole('button', { name: /reviews/i })
    expect(reviewsButtons.length).toBeGreaterThan(0)

    fireEvent.click(reviewsButtons[0])

    expect(screen.getByRole('link', { name: 'Flash Cards' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Review' })).toBeInTheDocument()
    expect(screen.queryByRole('link', { name: 'Writing' })).not.toBeInTheDocument()
  })

  it('renders Learning Path link with href /path when authenticated', () => {
    renderNavbar(authUser)

    const pathLinks = screen.getAllByRole('link', { name: 'Learning Path' })
    expect(pathLinks.length).toBeGreaterThan(0)
    expect(pathLinks[0]).toHaveAttribute('href', '/path')
  })

  it('renders profile avatar button with first letter of username when authenticated', () => {
    renderNavbar(authUser)

    const avatarButtons = screen.getAllByRole('button', { name: /profile menu/i })
    expect(avatarButtons.length).toBeGreaterThan(0)
    expect(avatarButtons[0]).toHaveTextContent('T')
  })

  it('shows Settings link and Logout button in profile dropdown', () => {
    renderNavbar(authUser)

    const avatarButtons = screen.getAllByRole('button', { name: /profile menu/i })
    fireEvent.click(avatarButtons[0])

    const settingsLinks = screen.getAllByRole('link', { name: /settings/i })
    expect(settingsLinks.length).toBeGreaterThan(0)
    expect(settingsLinks[0]).toHaveAttribute('href', '/settings')

    const logoutBtns = screen.getAllByRole('button', { name: /log out/i })
    expect(logoutBtns.length).toBeGreaterThan(0)
  })

  it('shows Admin link when user is admin', () => {
    renderNavbar(authAdmin)

    const adminLinks = screen.getAllByRole('link', { name: /^admin$/i })
    expect(adminLinks.length).toBeGreaterThan(0)
    expect(adminLinks[0]).toHaveAttribute('href', '/admin')
  })

  it('hides Admin link when user is not admin', () => {
    renderNavbar(authUser)

    expect(screen.queryByRole('link', { name: /^admin$/i })).not.toBeInTheDocument()
  })

  it('toggles mobile menu open and closed via hamburger button', () => {
    renderNavbar(authUser)

    const openButton = screen.getByRole('button', { name: /open menu/i })
    expect(openButton).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /close menu/i })).not.toBeInTheDocument()

    fireEvent.click(openButton)
    expect(screen.getByRole('button', { name: /close menu/i })).toBeInTheDocument()

    fireEvent.click(screen.getByRole('button', { name: /close menu/i }))
    expect(screen.queryByRole('button', { name: /close menu/i })).not.toBeInTheDocument()
  })

  it('opening Lessons dropdown closes Reviews dropdown', () => {
    renderNavbar(authUser)

    const reviewsButtons = screen.getAllByRole('button', { name: /reviews/i })
    fireEvent.click(reviewsButtons[0])
    expect(screen.getByRole('link', { name: 'Flash Cards' })).toBeInTheDocument()

    const lessonsButtons = screen.getAllByRole('button', { name: /lessons/i })
    fireEvent.click(lessonsButtons[0])
    expect(screen.getByRole('link', { name: 'Hiragana' })).toBeInTheDocument()

    expect(screen.queryByRole('link', { name: 'Flash Cards' })).not.toBeInTheDocument()
  })
})
