import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import KanjiListPage from '@/pages/KanjiListPage'

vi.mock('@/services/kanjiService', () => ({
  default: {
    getKanjiPaged: vi.fn(),
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

function makePagedResult(items: KanjiCharacter[], page = 1): PagedResult<KanjiCharacter> {
  return {
    items,
    totalCount: items.length,
    page,
    pageSize: 50,
    hasNextPage: false,
  }
}

describe('KanjiListPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders_level_selector_buttons', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    svc.getKanjiPaged.mockResolvedValue(makePagedResult([]))

    renderPage()

    expect(screen.getByRole('button', { name: 'All' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N5' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N4' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N3' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N2' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'N1' })).toBeInTheDocument()
  })

  it('renders_kanji_cards_when_data_loaded', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    svc.getKanjiPaged.mockResolvedValue(makePagedResult(sampleKanji))

    renderPage()

    expect(await screen.findByText('日')).toBeInTheDocument()
    expect(await screen.findByText('月')).toBeInTheDocument()
    expect(screen.getByText('sun, day')).toBeInTheDocument()
    expect(screen.getByText('moon, month')).toBeInTheDocument()
  })

  it('changes_level_on_button_click', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    svc.getKanjiPaged.mockResolvedValue(makePagedResult([]))

    renderPage()

    fireEvent.click(screen.getByRole('button', { name: 'N4' }))

    await vi.waitFor(() => {
      expect(svc.getKanjiPaged).toHaveBeenCalledWith(1, 4)
    })
  })

  it('shows_loading_state', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    // Never resolves during this test
    svc.getKanjiPaged.mockReturnValue(new Promise(() => {}))

    renderPage()

    expect(screen.getByText('Loading kanji...')).toBeInTheDocument()
  })

  it('shows_error_state_when_query_fails', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    svc.getKanjiPaged.mockRejectedValue(new Error('Network error'))

    renderPage()

    expect(await screen.findByText('Failed to load kanji.')).toBeInTheDocument()
  })

  it('renders_heading', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    svc.getKanjiPaged.mockResolvedValue(makePagedResult([]))

    renderPage()

    expect(screen.getByRole('heading', { name: 'Kanji' })).toBeInTheDocument()
  })

  it('kanji_card_shows_srs_stage', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    svc.getKanjiPaged.mockResolvedValue(makePagedResult(sampleKanji))

    renderPage()

    expect(await screen.findByText('Guru')).toBeInTheDocument()
    expect(await screen.findByText('Apprentice')).toBeInTheDocument()
  })

  it('defaults_to_All_level_on_initial_render', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    svc.getKanjiPaged.mockResolvedValue(makePagedResult([]))

    renderPage()

    await vi.waitFor(() => {
      expect(svc.getKanjiPaged).toHaveBeenCalledWith(1, null)
    })
  })

  it('shows_total_count_after_data_loads', async () => {
    const svc = kanjiService as unknown as { getKanjiPaged: ReturnType<typeof vi.fn> }
    svc.getKanjiPaged.mockResolvedValue(makePagedResult(sampleKanji))

    renderPage()

    expect(await screen.findByText('Showing 2 of 2 kanji')).toBeInTheDocument()
  })
})
