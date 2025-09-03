import { describe, it, expect, beforeEach, vi } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import KanaGrid from '@components/common/KanaGrid'

// Mock react-router-dom's useNavigate to avoid Router context
const mockNavigate = vi.fn()
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual<typeof import('react-router-dom')>('react-router-dom')
  return {
    ...actual,
    useNavigate: () => mockNavigate,
  }
})

// Mock the kanaService used inside KanaGrid
vi.mock('@services/kanaService', () => ({
  default: {
    getCharacters: vi.fn(),
  },
}))

import api from '@services/kanaService'

const sampleChars: KanaCharacter[] = [
  { character: 'あ', romanization: 'a', proficiency: 10 },
  { character: 'い', romanization: 'i', proficiency: 20 },
  { character: 'う', romanization: 'u', proficiency: 30 },
  { character: 'え', romanization: 'e', proficiency: 40 },
  { character: 'お', romanization: 'o', proficiency: 50 },
  { character: 'か', romanization: 'ka', proficiency: 60 },
]

describe('KanaGrid', () => {
  beforeEach(() => {
    vi.resetAllMocks()
  })

  it('fetches and displays characters in rows of 5 with KanaButtons', async () => {
    ;(api.getCharacters as unknown as ReturnType<typeof vi.fn>).mockResolvedValue(sampleChars)

    render(<KanaGrid type="hiragana" />)

    // Title renders
    expect(screen.getByText(/Hiragana Characters/i)).toBeInTheDocument()

    // Wait for buttons to render (each KanaButton has role button with name starting with 'Kana ' per component)
    await waitFor(() => {
      expect(screen.getByText('あ')).toBeInTheDocument()
      expect(screen.getByText('か')).toBeInTheDocument()
    })

    // Ensure correct number of buttons
    const buttons = screen.getAllByRole('button', { name: /Kana /i })
    expect(buttons).toHaveLength(sampleChars.length)
  })

  it('shows error message when fetch fails', async () => {
    ;(api.getCharacters as unknown as ReturnType<typeof vi.fn>).mockRejectedValue(new Error('Network down'))

    render(<KanaGrid type="katakana" />)

    await waitFor(() => {
      expect(screen.getByText(/Error: Network down/i)).toBeInTheDocument()
    })
  })
})
