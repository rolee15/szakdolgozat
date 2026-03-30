import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import KanjiListPage from '@/pages/KanjiListPage'

vi.mock('@/services/kanjiService', () => ({
  default: {
    getKanjiByLevel: vi.fn(),
    getKanjiDetail: vi.fn(),
  },
}))

import kanjiService from '@/services/kanjiService'

function makeQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
    },
  })
}

function renderPage() {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter>
        <KanjiListPage />
      </MemoryRouter>
    </QueryClientProvider>
  )
}

const sampleKanji: KanjiCharacter[] = [
  {
    character: '日',
    meaning: 'sun, day',
    onyomiReading: 'ニチ',
    kunyomiReading: 'ひ',
    jlptLevel: 5,
    strokeCount: 4,
    proficiency: 75,
    srsStage: 'Guru',
  },
  {
    character: '月',
    meaning: 'moon, month',
    onyomiReading: 'ゲツ',
    kunyomiReading: 'つき',
    jlptLevel: 5,
    strokeCount: 4,
    proficiency: 50,
    srsStage: 'Apprentice',
  },
]

describe('KanjiListPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders_level_selector_buttons', async () => {
    const svc = kanjiService as unknown as { getKanjiByLevel: ReturnType<typeof vi.fn> }
    svc.getKanjiByLevel.mockResolvedValue([])

    renderPage()

    expect(screen.getByRole('button', { name: 'N5' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N4' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N3' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N2' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N1' })).toBeInTheDocument()
  })

  it('renders_kanji_cards_when_data_loaded', async () => {
    const svc = kanjiService as unknown as { getKanjiByLevel: ReturnType<typeof vi.fn> }
    svc.getKanjiByLevel.mockResolvedValue(sampleKanji)

    renderPage()

    expect(await screen.findByText('日')).toBeInTheDocument()
    expect(await screen.findByText('月')).toBeInTheDocument()
    expect(screen.getByText('sun, day')).toBeInTheDocument()
    expect(screen.getByText('moon, month')).toBeInTheDocument()
  })

  it('changes_level_on_button_click', async () => {
    const svc = kanjiService as unknown as { getKanjiByLevel: ReturnType<typeof vi.fn> }
    svc.getKanjiByLevel.mockResolvedValue([])

    renderPage()

    fireEvent.click(screen.getByRole('button', { name: 'N4' }))

    await vi.waitFor(() => {
      expect(svc.getKanjiByLevel).toHaveBeenCalledWith(4)
    })
  })

  it('shows_loading_state', async () => {
    const svc = kanjiService as unknown as { getKanjiByLevel: ReturnType<typeof vi.fn> }
    // Never resolves during this test
    svc.getKanjiByLevel.mockReturnValue(new Promise(() => {}))

    renderPage()

    expect(screen.getByText('Loading kanji...')).toBeInTheDocument()
  })

  it('shows_error_state_when_query_fails', async () => {
    const svc = kanjiService as unknown as { getKanjiByLevel: ReturnType<typeof vi.fn> }
    svc.getKanjiByLevel.mockRejectedValue(new Error('Network error'))

    renderPage()

    expect(await screen.findByText('Failed to load kanji.')).toBeInTheDocument()
  })

  it('renders_heading', async () => {
    const svc = kanjiService as unknown as { getKanjiByLevel: ReturnType<typeof vi.fn> }
    svc.getKanjiByLevel.mockResolvedValue([])

    renderPage()

    expect(screen.getByRole('heading', { name: 'Kanji' })).toBeInTheDocument()
  })

  it('kanji_card_shows_srs_stage', async () => {
    const svc = kanjiService as unknown as { getKanjiByLevel: ReturnType<typeof vi.fn> }
    svc.getKanjiByLevel.mockResolvedValue(sampleKanji)

    renderPage()

    expect(await screen.findByText('Guru')).toBeInTheDocument()
    expect(await screen.findByText('Apprentice')).toBeInTheDocument()
  })

  it('defaults_to_N5_level_on_initial_render', async () => {
    const svc = kanjiService as unknown as { getKanjiByLevel: ReturnType<typeof vi.fn> }
    svc.getKanjiByLevel.mockResolvedValue([])

    renderPage()

    await vi.waitFor(() => {
      expect(svc.getKanjiByLevel).toHaveBeenCalledWith(5)
    })
  })
})
