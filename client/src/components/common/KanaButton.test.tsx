import React from 'react'
import { render, screen, fireEvent } from '@testing-library/react'
import KanaButton from './KanaButton'
import { vi } from 'vitest'

// Mock react-router-dom's useNavigate to capture navigation
const mockNavigate = vi.fn()
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual<typeof import('react-router-dom')>('react-router-dom')
  return {
    ...actual,
    useNavigate: () => mockNavigate,
  }
})

describe('KanaButton', () => {
  it('renders character and romanization', () => {
    render(<KanaButton type="hiragana" character="あ" romanization="a" proficiency={50} />)

    expect(screen.getByText('あ')).toBeInTheDocument()
    expect(screen.getByText('a')).toBeInTheDocument()

    // aria-label includes calculated level: floor(50/20)+1 = 3
    expect(screen.getByRole('button', { name: /Proficiency Level 3/i })).toBeInTheDocument()
  })

  it('clamps proficiency to [0,100] and reflects it in progress bar width and level', () => {
    const { rerender, container } = render(
      <KanaButton type="katakana" character="ア" romanization="a" proficiency={-10} />
    )

    // level should be 1 when clamped to 0
    expect(screen.getByRole('button', { name: /Proficiency Level 1/i })).toBeInTheDocument()

    // progress bar div is the inner div with style width
    const bars = container.querySelectorAll('div')
    const progress = Array.from(bars).find((el) => el.getAttribute('style')?.includes('width')) as HTMLDivElement
    expect(progress?.style.width).toBe('0%')

    rerender(<KanaButton type="katakana" character="ア" romanization="a" proficiency={250} />)
    // clamped to 100 -> level = floor(100/20)+1 = 6
    expect(screen.getByRole('button', { name: /Proficiency Level 6/i })).toBeInTheDocument()
    const progress2 = Array.from(container.querySelectorAll('div')).find((el) => el.getAttribute('style')?.includes('width')) as HTMLDivElement
    expect(progress2?.style.width).toBe('100%')
  })

  it('navigates to the correct route on click', () => {
    render(<KanaButton type="hiragana" character="か" romanization="ka" proficiency={20} />)

    const button = screen.getByRole('button', { name: /Kana か/i })
    fireEvent.click(button)

    expect(mockNavigate).toHaveBeenCalledWith('/hiragana/か')
  })
})
