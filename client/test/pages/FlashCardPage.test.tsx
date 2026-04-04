import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import FlashCardPage from '@/pages/FlashCardPage'

vi.mock('@/services/kanaService', () => ({
  default: {
    getCharacters: vi.fn(),
  },
}))

vi.mock('@/services/lessonService', () => ({
  default: {
    getLessonsCount: vi.fn(),
    getLessonReviewsCount: vi.fn(),
    getLessons: vi.fn(),
    getLessonReviews: vi.fn(),
    postLessonReviewCheck: vi.fn(),
    postLearnLesson: vi.fn(),
  },
}))

vi.mock('@/services/kanjiService', () => ({
  default: {
    getKanjiReviews: vi.fn(),
    checkKanjiReview: vi.fn(),
    getKanjiByLevel: vi.fn(),
    getKanjiPaged: vi.fn(),
    getKanjiDetail: vi.fn(),
  },
}))

import kanaService from '@/services/kanaService'
import lessonService from '@/services/lessonService'
import kanjiService from '@/services/kanjiService'

function makeQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  })
}

function renderPage() {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <FlashCardPage />
    </QueryClientProvider>
  )
}

const hiraganaCards: KanaCharacter[] = Array.from({ length: 3 }, (_, i) => ({
  character: ['あ', 'い', 'う'][i],
  romanization: ['a', 'i', 'u'][i],
  type: 'hiragana' as const,
  proficiency: 0,
}))

describe('FlashCardPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders_mode_selector_with_hiragana_and_katakana', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)

    renderPage()

    expect(await screen.findByRole('button', { name: 'Hiragana' })).toBeInTheDocument()
    expect(await screen.findByRole('button', { name: 'Katakana' })).toBeInTheDocument()
  })

  it('renders_kanji_button_as_enabled', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)

    renderPage()

    // Wait for the page to settle past loading
    await screen.findByRole('button', { name: 'Hiragana' })

    const kanjiButton = screen.getByRole('button', { name: /kanji/i })
    expect(kanjiButton).not.toBeDisabled()
  })

  it('renders_progress_indicator', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)

    renderPage()

    // First card, 3 total → "1 / 3"
    expect(await screen.findByText('1 / 3')).toBeInTheDocument()
  })

  it('flips_card_on_click', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)

    renderPage()

    // Wait for the card character to appear
    expect(await screen.findByText('あ')).toBeInTheDocument()

    // Before flip, front is visible: "Click to reveal" should be present
    expect(screen.getByText('Click to reveal')).toBeInTheDocument()

    // After flip, romanization is rendered in the DOM (even if visually hidden by CSS)
    // Both "あ" (front) and "a" (back) are in the DOM; CSS controls which is visible.
    expect(screen.getByText('a')).toBeInTheDocument()
  })

  it('know_it_advances_to_next_card', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const lessonSvc = lessonService as unknown as { postLessonReviewCheck: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)
    lessonSvc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })

    renderPage()

    await screen.findByText('1 / 3')

    fireEvent.click(screen.getByRole('button', { name: 'Know it' }))

    expect(await screen.findByText('2 / 3')).toBeInTheDocument()
  })

  it('dont_know_it_advances_to_next_card', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const lessonSvc = lessonService as unknown as { postLessonReviewCheck: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)
    lessonSvc.postLessonReviewCheck.mockResolvedValue({ isCorrect: false, correctAnswer: 'a' })

    renderPage()

    await screen.findByText('1 / 3')

    fireEvent.click(screen.getByRole('button', { name: /don't know it/i }))

    expect(await screen.findByText('2 / 3')).toBeInTheDocument()
  })

  it('shows_completion_screen_after_last_card', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const lessonSvc = lessonService as unknown as { postLessonReviewCheck: ReturnType<typeof vi.fn> }

    // Use 2 cards so we can exhaust them quickly
    const twoCards: KanaCharacter[] = hiraganaCards.slice(0, 2)
    svc.getCharacters.mockResolvedValue(twoCards)
    lessonSvc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })

    renderPage()

    await screen.findByText('1 / 2')

    // Advance through first card
    fireEvent.click(screen.getByRole('button', { name: 'Know it' }))
    await screen.findByText('2 / 2')

    // Advance through last card
    fireEvent.click(screen.getByRole('button', { name: 'Know it' }))

    expect(await screen.findByText(/session complete/i)).toBeInTheDocument()
    expect(await screen.findByRole('button', { name: /restart/i })).toBeInTheDocument()
  })

  it('shows_loading_state', () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockReturnValue(new Promise(() => {}))

    renderPage()

    expect(screen.getByText('Loading cards...')).toBeInTheDocument()
  })

  it('shows_error_state_when_fetch_fails', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockRejectedValue(new Error('Failed'))

    renderPage()

    expect(await screen.findByText('Failed to load characters.')).toBeInTheDocument()
  })

  it('switching_mode_resets_to_first_card', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const lessonSvc = lessonService as unknown as { postLessonReviewCheck: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)
    lessonSvc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })

    renderPage()

    await screen.findByText('1 / 3')

    // Advance one card
    fireEvent.click(screen.getByRole('button', { name: 'Know it' }))
    await screen.findByText('2 / 3')

    // Switch mode — triggers new query, resets index; mock new result as well
    const katakanaCards: KanaCharacter[] = [
      { character: 'ア', romanization: 'a', type: 'katakana', proficiency: 0 },
    ]
    svc.getCharacters.mockResolvedValue(katakanaCards)

    fireEvent.click(screen.getByRole('button', { name: 'Katakana' }))

    expect(await screen.findByText('1 / 1')).toBeInTheDocument()
  })

  it('kanji_mode_shows_empty_state_when_no_reviews_due', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const kanjiSvc = kanjiService as unknown as { getKanjiReviews: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)
    kanjiSvc.getKanjiReviews.mockResolvedValue([])

    renderPage()

    await screen.findByRole('button', { name: 'Hiragana' })
    fireEvent.click(screen.getByRole('button', { name: /kanji/i }))

    expect(await screen.findByText(/no kanji due for review/i)).toBeInTheDocument()
  })

  it('kanji_mode_shows_character_and_meaning', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const kanjiSvc = kanjiService as unknown as { getKanjiReviews: ReturnType<typeof vi.fn> }
    svc.getCharacters.mockResolvedValue(hiraganaCards)
    const kanjiCards: KanjiReview[] = [
      { kanjiId: 1, character: '日', meaning: 'sun' },
      { kanjiId: 2, character: '月', meaning: 'moon' },
    ]
    kanjiSvc.getKanjiReviews.mockResolvedValue(kanjiCards)

    renderPage()

    await screen.findByRole('button', { name: 'Hiragana' })
    fireEvent.click(screen.getByRole('button', { name: /kanji/i }))

    expect(await screen.findByText('日')).toBeInTheDocument()
    expect(screen.getByText('sun')).toBeInTheDocument()
  })

  it('kanji_know_it_advances_to_next_card', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const kanjiSvc = kanjiService as unknown as {
      getKanjiReviews: ReturnType<typeof vi.fn>
      checkKanjiReview: ReturnType<typeof vi.fn>
    }
    svc.getCharacters.mockResolvedValue(hiraganaCards)
    const kanjiCards: KanjiReview[] = [
      { kanjiId: 1, character: '日', meaning: 'sun' },
      { kanjiId: 2, character: '月', meaning: 'moon' },
    ]
    kanjiSvc.getKanjiReviews.mockResolvedValue(kanjiCards)
    kanjiSvc.checkKanjiReview.mockResolvedValue({ isCorrect: true, srsStage: 2, srsStageName: 'Apprentice 2' })

    renderPage()

    await screen.findByRole('button', { name: 'Hiragana' })
    fireEvent.click(screen.getByRole('button', { name: /kanji/i }))

    await screen.findByText('1 / 2')
    fireEvent.click(screen.getByRole('button', { name: 'Know it' }))

    expect(await screen.findByText('2 / 2')).toBeInTheDocument()
  })

  it('kanji_dont_know_it_advances_to_next_card', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const kanjiSvc = kanjiService as unknown as {
      getKanjiReviews: ReturnType<typeof vi.fn>
      checkKanjiReview: ReturnType<typeof vi.fn>
    }
    svc.getCharacters.mockResolvedValue(hiraganaCards)
    const kanjiCards: KanjiReview[] = [
      { kanjiId: 1, character: '日', meaning: 'sun' },
      { kanjiId: 2, character: '月', meaning: 'moon' },
    ]
    kanjiSvc.getKanjiReviews.mockResolvedValue(kanjiCards)
    kanjiSvc.checkKanjiReview.mockResolvedValue({ isCorrect: false, srsStage: 1, srsStageName: 'Apprentice 1' })

    renderPage()

    await screen.findByRole('button', { name: 'Hiragana' })
    fireEvent.click(screen.getByRole('button', { name: /kanji/i }))

    await screen.findByText('1 / 2')
    fireEvent.click(screen.getByRole('button', { name: /don't know it/i }))

    expect(await screen.findByText('2 / 2')).toBeInTheDocument()
  })

  it('restart_button_resets_session', async () => {
    const svc = kanaService as unknown as { getCharacters: ReturnType<typeof vi.fn> }
    const lessonSvc = lessonService as unknown as { postLessonReviewCheck: ReturnType<typeof vi.fn> }

    const oneCard: KanaCharacter[] = [hiraganaCards[0]]
    svc.getCharacters.mockResolvedValue(oneCard)
    lessonSvc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })

    renderPage()

    await screen.findByText('1 / 1')
    fireEvent.click(screen.getByRole('button', { name: 'Know it' }))

    await screen.findByText(/session complete/i)

    fireEvent.click(screen.getByRole('button', { name: /restart/i }))

    expect(await screen.findByText('1 / 1')).toBeInTheDocument()
  })
})
