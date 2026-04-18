import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

vi.mock('@/services/lessonService', () => ({
  default: {
    getLessonReviewsCount: vi.fn(),
    getLessonsCount: vi.fn(),
  },
}))

vi.mock('@/services/pathService', () => ({
  default: {
    getPath: vi.fn(),
    getUnitDetail: vi.fn(),
    getUnitTest: vi.fn(),
    submitTest: vi.fn(),
  },
}))

import HomePage from '@/pages/HomePage'
import lessonService from '@/services/lessonService'
import pathService from '@/services/pathService'

function makeQueryClient() {
  return new QueryClient({
    defaultOptions: { queries: { retry: false } },
  })
}

function renderPage() {
  return render(
    <QueryClientProvider client={makeQueryClient()}>
      <MemoryRouter>
        <HomePage />
      </MemoryRouter>
    </QueryClientProvider>
  )
}

const lessonSvc = lessonService as unknown as {
  getLessonReviewsCount: ReturnType<typeof vi.fn>
  getLessonsCount: ReturnType<typeof vi.fn>
}

const pathSvc = pathService as unknown as {
  getPath: ReturnType<typeof vi.fn>
}

describe('HomePage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders heading and description', async () => {
    lessonSvc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    lessonSvc.getLessonsCount.mockResolvedValue({ count: 0 })
    pathSvc.getPath.mockResolvedValue([])

    renderPage()

    expect(screen.getByRole('heading', { level: 1, name: /welcome to kanjika/i })).toBeInTheDocument()
    expect(screen.getByText('漢字家')).toBeInTheDocument()
  })

  it('shows review count when data loads', async () => {
    lessonSvc.getLessonReviewsCount.mockResolvedValue({ count: 7 })
    lessonSvc.getLessonsCount.mockResolvedValue({ count: 0 })
    pathSvc.getPath.mockResolvedValue([])

    renderPage()

    expect(await screen.findByText('7')).toBeInTheDocument()
    expect(screen.getByText('Due Reviews')).toBeInTheDocument()
  })

  it('shows lesson count when data loads', async () => {
    lessonSvc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    lessonSvc.getLessonsCount.mockResolvedValue({ count: 12 })
    pathSvc.getPath.mockResolvedValue([])

    renderPage()

    expect(await screen.findByText('12')).toBeInTheDocument()
    expect(screen.getByText('New Lessons')).toBeInTheDocument()
  })

  it('shows path progress when data loads', async () => {
    lessonSvc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    lessonSvc.getLessonsCount.mockResolvedValue({ count: 0 })
    pathSvc.getPath.mockResolvedValue([
      { id: 1, title: 'A', description: '', sortOrder: 1, contentCount: 1, isPassed: true, bestScore: 100, isUnlocked: true },
      { id: 2, title: 'B', description: '', sortOrder: 2, contentCount: 1, isPassed: true, bestScore: 80, isUnlocked: true },
      { id: 3, title: 'C', description: '', sortOrder: 3, contentCount: 1, isPassed: false, bestScore: 0, isUnlocked: true },
      { id: 4, title: 'D', description: '', sortOrder: 4, contentCount: 1, isPassed: false, bestScore: 0, isUnlocked: false },
      { id: 5, title: 'E', description: '', sortOrder: 5, contentCount: 1, isPassed: false, bestScore: 0, isUnlocked: false },
      { id: 6, title: 'F', description: '', sortOrder: 6, contentCount: 1, isPassed: false, bestScore: 0, isUnlocked: false },
    ])

    renderPage()

    expect(await screen.findByText('2 / 6 units completed')).toBeInTheDocument()
  })

  it('shows loading skeleton while data is loading', () => {
    // Never resolves
    lessonSvc.getLessonReviewsCount.mockReturnValue(new Promise(() => {}))
    lessonSvc.getLessonsCount.mockReturnValue(new Promise(() => {}))
    pathSvc.getPath.mockReturnValue(new Promise(() => {}))

    renderPage()

    const skeletons = document.querySelectorAll('.animate-pulse')
    expect(skeletons.length).toBeGreaterThan(0)
  })

  it('shows dash on error for review count', async () => {
    lessonSvc.getLessonReviewsCount.mockRejectedValue(new Error('fail'))
    lessonSvc.getLessonsCount.mockResolvedValue({ count: 3 })
    pathSvc.getPath.mockResolvedValue([])

    renderPage()

    expect(await screen.findByText('–')).toBeInTheDocument()
  })

  it('shows dash on error for lessons count', async () => {
    lessonSvc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    lessonSvc.getLessonsCount.mockRejectedValue(new Error('fail'))
    pathSvc.getPath.mockResolvedValue([])

    renderPage()

    expect(await screen.findByText('–')).toBeInTheDocument()
  })

  it('shows dash on error for path query', async () => {
    lessonSvc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    lessonSvc.getLessonsCount.mockResolvedValue({ count: 0 })
    pathSvc.getPath.mockRejectedValue(new Error('fail'))

    renderPage()

    expect(await screen.findByText('–')).toBeInTheDocument()
  })

  it('stat cards link to correct routes', async () => {
    lessonSvc.getLessonReviewsCount.mockResolvedValue({ count: 1 })
    lessonSvc.getLessonsCount.mockResolvedValue({ count: 2 })
    pathSvc.getPath.mockResolvedValue([])

    renderPage()

    expect(await screen.findByText('1')).toBeInTheDocument()

    const reviewLink = screen.getByRole('link', { name: /due reviews/i })
    expect(reviewLink).toHaveAttribute('href', '/lessons/reviews')

    const lessonsLink = screen.getByRole('link', { name: /new lessons/i })
    expect(lessonsLink).toHaveAttribute('href', '/lessons')

    const pathLink = screen.getByRole('link', { name: /path progress/i })
    expect(pathLink).toHaveAttribute('href', '/path')
  })
})
