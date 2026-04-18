import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import SrsBadge from '@/components/common/SrsBadge'

describe('SrsBadge', () => {
  it('renders the stage text', () => {
    render(<SrsBadge stage="Master" />)
    expect(screen.getByText('Master')).toBeInTheDocument()
  })

  it('applies bg-pink-600 for Apprentice 1', () => {
    render(<SrsBadge stage="Apprentice 1" />)
    expect(screen.getByText('Apprentice 1').className).toContain('bg-pink-600')
  })

  it('applies bg-pink-600 for Apprentice 2', () => {
    render(<SrsBadge stage="Apprentice 2" />)
    expect(screen.getByText('Apprentice 2').className).toContain('bg-pink-600')
  })

  it('applies bg-purple-600 for Guru 1', () => {
    render(<SrsBadge stage="Guru 1" />)
    expect(screen.getByText('Guru 1').className).toContain('bg-purple-600')
  })

  it('applies bg-purple-600 for Guru 2', () => {
    render(<SrsBadge stage="Guru 2" />)
    expect(screen.getByText('Guru 2').className).toContain('bg-purple-600')
  })

  it('applies bg-blue-600 for Master', () => {
    render(<SrsBadge stage="Master" />)
    expect(screen.getByText('Master').className).toContain('bg-blue-600')
  })

  it('applies bg-cyan-600 for Enlightened', () => {
    render(<SrsBadge stage="Enlightened" />)
    expect(screen.getByText('Enlightened').className).toContain('bg-cyan-600')
  })

  it('applies bg-amber-600 for Burned', () => {
    render(<SrsBadge stage="Burned" />)
    expect(screen.getByText('Burned').className).toContain('bg-amber-600')
  })

  it('applies bg-gray-600 for Locked', () => {
    render(<SrsBadge stage="Locked" />)
    expect(screen.getByText('Locked').className).toContain('bg-gray-600')
  })

  it('applies bg-gray-600 for unknown stage', () => {
    render(<SrsBadge stage="Unknown Stage" />)
    expect(screen.getByText('Unknown Stage').className).toContain('bg-gray-600')
  })
})
