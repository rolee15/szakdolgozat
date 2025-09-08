import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import Logo from '@/components/layout/Logo'

describe('Logo', () => {
  it('renders link to home with image alt and brand text', () => {
    render(
      <MemoryRouter>
        <Logo />
      </MemoryRouter>
    )

    const link = screen.getByRole('link')
    expect(link).toHaveAttribute('href', '/')
    expect(screen.getByAltText('Vite logo')).toBeInTheDocument()
    expect(screen.getByText('KanjiKa')).toBeInTheDocument()
  })
})
