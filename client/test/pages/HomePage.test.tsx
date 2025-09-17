import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import HomePage from '@/pages/HomePage'

describe('HomePage', () => {
  it('renders heading and description', () => {
    render(<HomePage />)
    expect(screen.getByRole('heading', { level: 1, name: /welcome to kanjika/i })).toBeInTheDocument()
    expect(screen.getByText('漢字家')).toBeInTheDocument()
  })
})
