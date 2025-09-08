import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import ErrorPage from '@/pages/ErrorPage'

describe('ErrorPage', () => {
  it('renders message and link to home', () => {
    render(
      <MemoryRouter>
        <ErrorPage />
      </MemoryRouter>
    )
    expect(screen.getByText(/page not found/i)).toBeInTheDocument()
    const link = screen.getByRole('link', { name: /return to home/i })
    expect(link).toHaveAttribute('href', '/')
  })
})
