import { describe, it, expect, vi } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import Navbar from '@/components/layout/Navbar'

vi.mock('@/components/layout/Logo', () => ({ default: () => <div data-testid="logo">Logo</div> }))

const mockUseAuth = vi.fn()
vi.mock('@/context/AuthContext', () => ({
  useAuth: () => mockUseAuth(),
}))

const defaultAuth = { isAuthenticated: false, username: null, isAdmin: false, logout: vi.fn() }

function renderNavbar(auth = defaultAuth) {
  mockUseAuth.mockReturnValue(auth)
  return render(
    <MemoryRouter>
      <Navbar />
    </MemoryRouter>
  )
}

describe('Navbar', () => {
  // 1. Logo renders
  it('renders the Logo', () => {
    renderNavbar()
    expect(screen.getByTestId('logo')).toBeInTheDocument()
  })

  // 2. Study dropdown shows links when clicked
  it('shows Study dropdown links when Study button is clicked', () => {
    renderNavbar()

    const studyButtons = screen.getAllByRole('button', { name: /study/i })
    expect(studyButtons.length).toBeGreaterThan(0)

    fireEvent.click(studyButtons[0])

    expect(screen.getByRole('link', { name: 'Hiragana' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Katakana' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Kanji' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Grammar' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Reading' })).toBeInTheDocument()
  })

  // 3. Practice dropdown shows links when clicked
  it('shows Practice dropdown links when Practice button is clicked', () => {
    renderNavbar()

    const practiceButtons = screen.getAllByRole('button', { name: /practice/i })
    expect(practiceButtons.length).toBeGreaterThan(0)

    fireEvent.click(practiceButtons[0])

    expect(screen.getAllByRole('link', { name: 'Lessons' })[0]).toBeInTheDocument()
    expect(screen.getAllByRole('link', { name: 'Writing' })[0]).toBeInTheDocument()
    expect(screen.getAllByRole('link', { name: 'Flash Cards' })[0]).toBeInTheDocument()
  })

  // 4. Writing link has correct href /lessons/writing
  it('Writing link in Practice dropdown has href /lessons/writing', () => {
    renderNavbar()

    const practiceButtons = screen.getAllByRole('button', { name: /practice/i })
    fireEvent.click(practiceButtons[0])

    const writingLinks = screen.getAllByRole('link', { name: 'Writing' })
    expect(writingLinks[0]).toHaveAttribute('href', '/lessons/writing')
  })

  // 5. Learning Path link renders with href /path
  it('renders the Learning Path link with href /path', () => {
    renderNavbar()

    const pathLinks = screen.getAllByRole('link', { name: 'Learning Path' })
    expect(pathLinks.length).toBeGreaterThan(0)
    expect(pathLinks[0]).toHaveAttribute('href', '/path')
  })

  // 6. Auth — not authenticated: Login and Register links visible
  it('shows Login and Register links when not authenticated', () => {
    renderNavbar()

    const loginLinks = screen.getAllByRole('link', { name: /login/i })
    const registerLinks = screen.getAllByRole('link', { name: /register/i })

    expect(loginLinks.length).toBeGreaterThan(0)
    expect(loginLinks[0]).toHaveAttribute('href', '/login')

    expect(registerLinks.length).toBeGreaterThan(0)
    expect(registerLinks[0]).toHaveAttribute('href', '/register')
  })

  // 7. Auth — authenticated: username, Settings, Logout
  it('shows username, Settings link, and Logout button when authenticated', () => {
    renderNavbar({ isAuthenticated: true, username: 'testuser', isAdmin: false, logout: vi.fn() })

    const usernameEls = screen.getAllByText('testuser')
    expect(usernameEls.length).toBeGreaterThan(0)

    const settingsLinks = screen.getAllByRole('link', { name: /settings/i })
    expect(settingsLinks.length).toBeGreaterThan(0)
    expect(settingsLinks[0]).toHaveAttribute('href', '/settings')

    const logoutBtns = screen.getAllByRole('button', { name: /logout/i })
    expect(logoutBtns.length).toBeGreaterThan(0)
  })

  // 8a. Admin link shows when isAdmin: true
  it('shows Admin link when user is admin', () => {
    renderNavbar({ isAuthenticated: true, username: 'adminuser', isAdmin: true, logout: vi.fn() })

    const adminLinks = screen.getAllByRole('link', { name: /^admin$/i })
    expect(adminLinks.length).toBeGreaterThan(0)
    expect(adminLinks[0]).toHaveAttribute('href', '/admin')
  })

  // 8b. Admin link hidden when isAdmin: false
  it('hides Admin link when user is not admin', () => {
    renderNavbar({ isAuthenticated: true, username: 'regularuser', isAdmin: false, logout: vi.fn() })

    expect(screen.queryByRole('link', { name: /^admin$/i })).not.toBeInTheDocument()
  })

  // 9. Mobile hamburger toggles menu open/close
  it('toggles mobile menu open and closed via hamburger button', () => {
    renderNavbar()

    const openButton = screen.getByRole('button', { name: /open menu/i })
    expect(openButton).toBeInTheDocument()
    expect(screen.queryByRole('button', { name: /close menu/i })).not.toBeInTheDocument()

    fireEvent.click(openButton)

    expect(screen.getByRole('button', { name: /close menu/i })).toBeInTheDocument()

    fireEvent.click(screen.getByRole('button', { name: /close menu/i }))

    expect(screen.queryByRole('button', { name: /close menu/i })).not.toBeInTheDocument()
  })

  // 10. Clicking a mobile nav link closes the menu
  it('closes mobile menu when a link is clicked', () => {
    renderNavbar()

    fireEvent.click(screen.getByRole('button', { name: /open menu/i }))
    expect(screen.getByRole('button', { name: /close menu/i })).toBeInTheDocument()

    // Learning Path only appears in mobile nav after opening, click it
    const pathLinks = screen.getAllByRole('link', { name: 'Learning Path' })
    fireEvent.click(pathLinks[pathLinks.length - 1])

    expect(screen.queryByRole('button', { name: /close menu/i })).not.toBeInTheDocument()
  })

  // 11. Opening Study dropdown closes Practice dropdown
  it('opening Study dropdown closes Practice dropdown', () => {
    renderNavbar()

    const practiceButtons = screen.getAllByRole('button', { name: /practice/i })
    fireEvent.click(practiceButtons[0])

    // Practice dropdown is open — Lessons link visible
    expect(screen.getAllByRole('link', { name: 'Lessons' })[0]).toBeInTheDocument()

    // Now open Study — Practice should close
    const studyButtons = screen.getAllByRole('button', { name: /study/i })
    fireEvent.click(studyButtons[0])

    // Study links now visible
    expect(screen.getByRole('link', { name: 'Hiragana' })).toBeInTheDocument()

    // Practice dropdown links should be gone (no Lessons in desktop dropdown)
    // Lessons still appears in mobile nav section (which is hidden by CSS, but rendered)
    // We verify Practice dropdown panel is closed by checking Flash Cards is absent
    // (Flash Cards only appears inside Practice dropdown panel, not mobile by default)
    expect(screen.queryByRole('link', { name: 'Flash Cards' })).not.toBeInTheDocument()
  })
})
