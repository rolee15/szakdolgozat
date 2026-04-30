import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'

import HomePage from '@/pages/HomePage'

function renderPage() {
  return render(
    <MemoryRouter>
      <HomePage />
    </MemoryRouter>
  )
}

describe('HomePage', () => {
  it('renders heading and description', () => {
    renderPage()

    expect(screen.getByRole('heading', { level: 1, name: 'Kanjika' })).toBeInTheDocument()
    expect(screen.getByRole('heading', { level: 1, name: '漢字家' })).toBeInTheDocument()
    expect(screen.getByText(/welcome to kanjika/i)).toBeInTheDocument()
    expect(screen.getByText('漢字家へようこそ！')).toBeInTheDocument()
    expect(screen.getByText(/where you can learn and practice japanese/i)).toBeInTheDocument()
    expect(screen.getByText('日本語を学び、練習できる場所です。')).toBeInTheDocument()
  })
})
