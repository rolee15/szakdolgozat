import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import Layout from '@/components/layout/Layout'

// Mock child components to focus on structure
vi.mock('@/components/layout/Navbar', () => ({ default: () => <div data-testid="navbar">Navbar</div> }))
vi.mock('@/components/layout/Footer', () => ({ default: () => <div data-testid="footer">Footer</div> }))

function Child() {
  return <div>Child Content</div>
}

describe('Layout', () => {
  it('renders Navbar, Outlet content, and Footer', () => {
    render(
      <MemoryRouter initialEntries={['/']}> 
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route index element={<Child />} />
          </Route>
        </Routes>
      </MemoryRouter>
    )

    expect(screen.getByTestId('navbar')).toBeInTheDocument()
    expect(screen.getByText('Child Content')).toBeInTheDocument()
    expect(screen.getByTestId('footer')).toBeInTheDocument()
  })
})
