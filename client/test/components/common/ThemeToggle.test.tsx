import { describe, it, expect, beforeEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import ThemeToggle from '@/components/common/ThemeToggle'
import { ThemeProvider } from '@/context/ThemeContext'

function renderToggle() {
  return render(
    <ThemeProvider>
      <ThemeToggle />
    </ThemeProvider>
  )
}

describe('ThemeToggle', () => {
  beforeEach(() => {
    localStorage.clear()
    document.documentElement.classList.remove('dark')
  })

  it('renders a radiogroup with two theme options', () => {
    renderToggle()

    expect(screen.getByRole('radiogroup', { name: /theme/i })).toBeInTheDocument()
    expect(screen.getByRole('radio', { name: /light mode/i })).toBeInTheDocument()
    expect(screen.getByRole('radio', { name: /dark mode/i })).toBeInTheDocument()
  })

  it('checks the light radio when current theme is light', () => {
    renderToggle()

    expect(screen.getByRole('radio', { name: /light mode/i })).toBeChecked()
    expect(screen.getByRole('radio', { name: /dark mode/i })).not.toBeChecked()
  })

  it('checks the dark radio when current theme is dark', () => {
    localStorage.setItem('theme', 'dark')
    renderToggle()

    expect(screen.getByRole('radio', { name: /dark mode/i })).toBeChecked()
    expect(screen.getByRole('radio', { name: /light mode/i })).not.toBeChecked()
  })

  it('switches to dark when the dark radio is selected', () => {
    renderToggle()

    fireEvent.click(screen.getByRole('radio', { name: /dark mode/i }))

    expect(screen.getByRole('radio', { name: /dark mode/i })).toBeChecked()
    expect(screen.getByRole('radio', { name: /light mode/i })).not.toBeChecked()
    expect(document.documentElement.classList.contains('dark')).toBe(true)
    expect(localStorage.getItem('theme')).toBe('dark')
  })

  it('switches back to light when the light radio is selected', () => {
    localStorage.setItem('theme', 'dark')
    renderToggle()

    fireEvent.click(screen.getByRole('radio', { name: /light mode/i }))

    expect(screen.getByRole('radio', { name: /light mode/i })).toBeChecked()
    expect(document.documentElement.classList.contains('dark')).toBe(false)
    expect(localStorage.getItem('theme')).toBe('light')
  })

  it('renders sun and moon icons inside the radio labels', () => {
    const { container } = renderToggle()

    // Two SVGs (sun + moon) wrapped in the labels
    const svgs = container.querySelectorAll('svg')
    expect(svgs.length).toBe(2)
  })
})
