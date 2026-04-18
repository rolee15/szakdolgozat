import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import AdminDashboardPage from '@/pages/admin/AdminDashboardPage'

describe('AdminDashboardPage', () => {
  it('renders Admin Dashboard heading', () => {
    render(
      <MemoryRouter>
        <AdminDashboardPage />
      </MemoryRouter>
    )

    expect(screen.getByRole('heading', { name: /admin dashboard/i })).toBeInTheDocument()
  })

  it('renders link to user management', () => {
    render(
      <MemoryRouter>
        <AdminDashboardPage />
      </MemoryRouter>
    )

    const link = screen.getByRole('link', { name: /user management/i })
    expect(link).toBeInTheDocument()
    expect(link).toHaveAttribute('href', '/admin/users')
  })
})
