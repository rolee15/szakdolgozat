import { describe, it, expect, beforeEach, vi } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import CharacterDetail from '@components/common/CharacterDetail'

// Mock route params for useParams
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual<typeof import('react-router-dom')>('react-router-dom')
  return {
    ...actual,
    useParams: () => ({ type: 'hiragana', character: 'あ' }),
  }
})

// Mock services called by CharacterDetail
vi.mock('@services/kanaService', () => ({
  default: {
    getCharacterDetail: vi.fn(),
    getExamples: vi.fn(),
  },
}))

import api from '@services/kanaService'

describe('CharacterDetail', () => {
  beforeEach(() => {
    vi.resetAllMocks()
  })

  it('loads and displays character details and examples', async () => {
    ;(api.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).mockResolvedValue({
      character: 'あ',
      romanization: 'a',
      proficiency: 80,
    } satisfies KanaCharacter)

    ;(api.getExamples as unknown as ReturnType<typeof vi.fn>).mockResolvedValue([
      { word: 'あい', romanization: 'ai', meaning: 'love' },
      { word: 'あお', romanization: 'ao', meaning: 'blue' },
    ] satisfies Example[])

    render(<CharacterDetail />)

    // Initially shows Loading...
    expect(screen.getByText(/Loading/i)).toBeInTheDocument()

    // Wait for the character and romaji to appear
    await waitFor(() => {
      expect(screen.getByText('あ')).toBeInTheDocument()
      expect(screen.getByText('a')).toBeInTheDocument()
      expect(screen.getByText(/Proficiency: 80%/i)).toBeInTheDocument()
    })

    // Example words
    expect(screen.getByText(/Example Words/i)).toBeInTheDocument()
    expect(screen.getByText(/あい \(ai\) - love/)).toBeInTheDocument()
    expect(screen.getByText(/あお \(ao\) - blue/)).toBeInTheDocument()
  })

  it('shows error message when fetching fails', async () => {
    ;(api.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).mockRejectedValue(new Error('bad request'))
    ;(api.getExamples as unknown as ReturnType<typeof vi.fn>).mockResolvedValue([])

    render(<CharacterDetail />)

    await waitFor(() => {
      expect(screen.getByText(/Error: bad request/i)).toBeInTheDocument()
    })
  })
})
