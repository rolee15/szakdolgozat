import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter, Route, Routes } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import KanjiDetailPage from '@/pages/KanjiDetailPage'

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

function renderPage(character: string = '日') {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter initialEntries={[`/kanji/${character}`]}>
        <Routes>
          <Route path="/kanji/:character" element={<KanjiDetailPage />} />
        </Routes>
      </MemoryRouter>
    </QueryClientProvider>
  )
}

const sampleKanjiDetail: KanjiDetail = {
  character: '日',
  meaning: 'sun, day',
  onyomiReading: 'ニチ、ジツ',
  kunyomiReading: 'ひ、-び、-か',
  strokeCount: 4,
  jlptLevel: 5,
  grade: 1,
  proficiency: 75,
  srsStage: 'Guru',
  examples: [
    { word: '日本', reading: 'にほん', meaning: 'Japan' },
    { word: '日曜日', reading: 'にちようび', meaning: 'Sunday' },
  ],
}

describe('KanjiDetailPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders_kanji_character_and_meaning', async () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockResolvedValue(sampleKanjiDetail)

    renderPage()

    expect(await screen.findByText('日')).toBeInTheDocument()
    expect(await screen.findByText('sun, day')).toBeInTheDocument()
  })

  it('renders_readings', async () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockResolvedValue(sampleKanjiDetail)

    renderPage()

    expect(await screen.findByText('ニチ、ジツ')).toBeInTheDocument()
    expect(await screen.findByText('ひ、-び、-か')).toBeInTheDocument()
  })

  it('renders_examples_table', async () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockResolvedValue(sampleKanjiDetail)

    renderPage()

    expect(await screen.findByText('日本')).toBeInTheDocument()
    expect(await screen.findByText('にほん')).toBeInTheDocument()
    expect(await screen.findByText('Japan')).toBeInTheDocument()
    expect(await screen.findByText('日曜日')).toBeInTheDocument()
    expect(await screen.findByText('Sunday')).toBeInTheDocument()
  })

  it('shows_loading_state', () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockReturnValue(new Promise(() => {}))

    renderPage()

    expect(screen.getByText('Loading...')).toBeInTheDocument()
  })

  it('shows_error_state_when_query_fails', async () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockRejectedValue(new Error('Not found'))

    renderPage()

    expect(await screen.findByText('Failed to load kanji details.')).toBeInTheDocument()
  })

  it('renders_stroke_count_and_jlpt_level', async () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockResolvedValue(sampleKanjiDetail)

    renderPage()

    expect(await screen.findByText('4')).toBeInTheDocument()
    expect(await screen.findByText('N5')).toBeInTheDocument()
  })

  it('renders_proficiency_percentage', async () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockResolvedValue(sampleKanjiDetail)

    renderPage()

    expect(await screen.findByText('75%')).toBeInTheDocument()
  })

  it('renders_dash_for_missing_readings', async () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockResolvedValue({
      ...sampleKanjiDetail,
      onyomiReading: '',
      kunyomiReading: '',
    })

    renderPage()

    const dashes = await screen.findAllByText('—')
    expect(dashes.length).toBeGreaterThanOrEqual(2)
  })

  it('does_not_render_examples_section_when_empty', async () => {
    const svc = kanjiService as unknown as { getKanjiDetail: ReturnType<typeof vi.fn> }
    svc.getKanjiDetail.mockResolvedValue({ ...sampleKanjiDetail, examples: [] })

    renderPage()

    // Wait for the detail to load
    await screen.findByText('sun, day')

    expect(screen.queryByText('Examples')).not.toBeInTheDocument()
  })
})
