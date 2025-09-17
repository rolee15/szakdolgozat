import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import Navbar from '@/components/layout/Navbar'

// Mock Logo to simplify
vi.mock('@/components/layout/Logo', () => ({ default: () => <div data-testid="logo">Logo</div> }))

describe('Navbar', () => {
  it('renders Logo, menu items, and auth buttons with proper links', () => {
    render(
      <MemoryRouter>
        <Navbar />
      </MemoryRouter>
    )

    expect(screen.getByTestId('logo')).toBeInTheDocument()

    // Menu items
    expect(screen.getByRole('link', { name: 'Hiragana' })).toHaveAttribute('href', '/hiragana')
    expect(screen.getByRole('link', { name: 'Katakana' })).toHaveAttribute('href', '/katakana')
    expect(screen.getByRole('link', { name: 'Lessons' })).toHaveAttribute('href', '/lessons')

    // Auth buttons are inside links
    const loginLink = screen.getByRole('link', { name: /login/i })
    expect(loginLink).toHaveAttribute('href', '/login')

    const registerLink = screen.getByRole('link', { name: /register/i })
    expect(registerLink).toHaveAttribute('href', '/register')
  })
})
