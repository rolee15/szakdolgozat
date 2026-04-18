import { describe, it, expect, beforeEach, vi } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import CharacterDetail from '@components/common/CharacterDetail'

// Mock services called by CharacterDetail
vi.mock('@/services/hiraganaService', () => ({
  default: {
    getCharacterDetail: vi.fn(),
    getExamples: vi.fn(),
  },
}))

vi.mock('@/services/katakanaService', () => ({
  default: {
    getCharacterDetail: vi.fn(),
    getExamples: vi.fn(),
  },
}))

import hiraganaApi from '@/services/hiraganaService'
import katakanaApi from '@/services/katakanaService'

let mockType = 'hiragana'
let mockCharacter = 'あ'

vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual<typeof import('react-router-dom')>('react-router-dom')
  return {
    ...actual,
    useParams: () => ({ type: mockType, character: mockCharacter }),
  }
})

describe('CharacterDetail', () => {
  beforeEach(() => {
    vi.resetAllMocks()
    mockType = 'hiragana'
    mockCharacter = 'あ'
  })

  it('loads and displays hiragana character details and examples', async () => {
    ;(hiraganaApi.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).mockResolvedValue({
      character: 'あ',
      romanization: 'a',
      type: 'hiragana',
      proficiency: 80,
    } satisfies KanaCharacter)

    ;(hiraganaApi.getExamples as unknown as ReturnType<typeof vi.fn>).mockResolvedValue([
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

  it('loads and displays katakana character details and examples', async () => {
    mockType = 'katakana'
    mockCharacter = 'ア'

    ;(katakanaApi.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).mockResolvedValue({
      character: 'ア',
      romanization: 'a',
      type: 'katakana',
      proficiency: 50,
    } satisfies KanaCharacter)

    ;(katakanaApi.getExamples as unknown as ReturnType<typeof vi.fn>).mockResolvedValue([
      { word: 'アイス', romanization: 'aisu', meaning: 'ice cream' },
    ] satisfies Example[])

    render(<CharacterDetail />)

    await waitFor(() => {
      expect(screen.getByText('ア')).toBeInTheDocument()
      expect(screen.getByText(/Proficiency: 50%/i)).toBeInTheDocument()
    })

    expect(screen.getByText(/アイス \(aisu\) - ice cream/)).toBeInTheDocument()
  })

  it('shows error message when fetching fails', async () => {
    ;(hiraganaApi.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).mockRejectedValue(new Error('bad request'))
    ;(hiraganaApi.getExamples as unknown as ReturnType<typeof vi.fn>).mockResolvedValue([])

    render(<CharacterDetail />)

    await waitFor(() => {
      expect(screen.getByText(/Error: bad request/i)).toBeInTheDocument()
    })
  })

  it('shows SRS stage badge when srsStageName is defined', async () => {
    ;(hiraganaApi.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).mockResolvedValue({
      character: 'あ',
      romanization: 'a',
      type: 'hiragana',
      proficiency: 90,
      srsStageName: 'Guru',
    } satisfies KanaCharacter)

    ;(hiraganaApi.getExamples as unknown as ReturnType<typeof vi.fn>).mockResolvedValue([])

    render(<CharacterDetail />)

    await waitFor(() => {
      expect(screen.getByText(/SRS Stage/i)).toBeInTheDocument()
      expect(screen.getByText('Guru')).toBeInTheDocument()
    })
  })

  it('shows error message when a non-Error is thrown', async () => {
    ;(hiraganaApi.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).mockRejectedValue('string error')
    ;(hiraganaApi.getExamples as unknown as ReturnType<typeof vi.fn>).mockResolvedValue([])

    render(<CharacterDetail />)

    await waitFor(() => {
      expect(screen.getByText(/Error: Failed to load character data/i)).toBeInTheDocument()
    })
  })

  it('does nothing when type or character params are missing', async () => {
    mockType = ''
    mockCharacter = ''

    render(<CharacterDetail />)

    // Should stay in loading state with no API calls made
    expect(screen.getByText(/Loading/i)).toBeInTheDocument()
    expect(hiraganaApi.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).not.toHaveBeenCalled()
    expect(katakanaApi.getCharacterDetail as unknown as ReturnType<typeof vi.fn>).not.toHaveBeenCalled()
  })
})
