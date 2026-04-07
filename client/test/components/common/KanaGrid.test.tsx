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

// Mock the services used inside KanaGrid
vi.mock('@/services/hiraganaService', () => ({
  default: {
    getCharacters: vi.fn(),
  },
}))

vi.mock('@/services/katakanaService', () => ({
  default: {
    getCharacters: vi.fn(),
  },
}))

import hiraganaApi from '@/services/hiraganaService'
import katakanaApi from '@/services/katakanaService'

const hiraganaChars: KanaCharacter[] = [
  { character: 'あ', romanization: 'a', type: 'hiragana', proficiency: 10 },
  { character: 'い', romanization: 'i', type: 'hiragana', proficiency: 20 },
  { character: 'う', romanization: 'u', type: 'hiragana', proficiency: 30 },
  { character: 'え', romanization: 'e', type: 'hiragana', proficiency: 40 },
  { character: 'お', romanization: 'o', type: 'hiragana', proficiency: 50 },
  { character: 'か', romanization: 'ka', type: 'hiragana', proficiency: 60 },
]

describe('KanaGrid', () => {
  beforeEach(() => {
    vi.resetAllMocks()
  })

  it('fetches hiragana characters using hiraganaService', async () => {
    ;(hiraganaApi.getCharacters as unknown as ReturnType<typeof vi.fn>).mockResolvedValue(hiraganaChars)

    render(<KanaGrid type="hiragana" />)

    expect(screen.getByText(/Hiragana Characters/i)).toBeInTheDocument()

    await waitFor(() => {
      expect(screen.getByText('あ')).toBeInTheDocument()
      expect(screen.getByText('か')).toBeInTheDocument()
    })

    const buttons = screen.getAllByRole('button', { name: /Kana /i })
    expect(buttons).toHaveLength(hiraganaChars.length)
    expect(hiraganaApi.getCharacters as unknown as ReturnType<typeof vi.fn>).toHaveBeenCalled()
    expect(katakanaApi.getCharacters as unknown as ReturnType<typeof vi.fn>).not.toHaveBeenCalled()
  })

  it('fetches katakana characters using katakanaService', async () => {
    const katakanaChars: KanaCharacter[] = [
      { character: 'ア', romanization: 'a', type: 'katakana', proficiency: 10 },
    ]
    ;(katakanaApi.getCharacters as unknown as ReturnType<typeof vi.fn>).mockResolvedValue(katakanaChars)

    render(<KanaGrid type="katakana" />)

    expect(screen.getByText(/Katakana Characters/i)).toBeInTheDocument()

    await waitFor(() => {
      expect(screen.getByText('ア')).toBeInTheDocument()
    })

    expect(katakanaApi.getCharacters as unknown as ReturnType<typeof vi.fn>).toHaveBeenCalled()
    expect(hiraganaApi.getCharacters as unknown as ReturnType<typeof vi.fn>).not.toHaveBeenCalled()
  })

  it('shows error message when fetch fails with an Error', async () => {
    ;(hiraganaApi.getCharacters as unknown as ReturnType<typeof vi.fn>).mockRejectedValue(new Error('Network down'))

    render(<KanaGrid type="hiragana" />)

    await waitFor(() => {
      expect(screen.getByText(/Error: Network down/i)).toBeInTheDocument()
    })
  })

  it('shows fallback error message when a non-Error is thrown', async () => {
    ;(hiraganaApi.getCharacters as unknown as ReturnType<typeof vi.fn>).mockRejectedValue('unexpected')

    render(<KanaGrid type="hiragana" />)

    await waitFor(() => {
      expect(screen.getByText(/Error: Failed to load characters/i)).toBeInTheDocument()
    })
  })
})
