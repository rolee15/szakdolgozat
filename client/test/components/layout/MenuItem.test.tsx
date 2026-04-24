import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import MenuItem from '@/components/layout/MenuItem'

describe('MenuItem', () => {
  it('renders NavLink to the given path with the given text', () => {
    render(
      <MemoryRouter>
        <MenuItem to="/hiragana">Hiragana</MenuItem>
      </MemoryRouter>
    )

    const link = screen.getByRole('link', { name: 'Hiragana' })
    expect(link).toHaveAttribute('href', '/hiragana')
  })
})
