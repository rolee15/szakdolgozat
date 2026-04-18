import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import LessonsPage from '@/pages/LessonsPage'

vi.mock('@/services/lessonService', () => ({
  default: {
    getLessonsCount: vi.fn(),
    getLessonReviewsCount: vi.fn(),
    getWritingReviewsCount: vi.fn(),
  }
}))

import lessonService from '@/services/lessonService'

describe('LessonsPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })
  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders counts and links to learn and review', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockResolvedValue({ count: 5 })
    svc.getLessonReviewsCount.mockResolvedValue({ count: 3 })
    svc.getWritingReviewsCount.mockResolvedValue({ count: 2 })

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    // await counts to appear
    expect(await screen.findByText('5')).toBeInTheDocument()
    expect(await screen.findByText('3')).toBeInTheDocument()

    const learnLink = screen.getByRole('link', { name: /learn/i })
    expect(learnLink).toHaveAttribute('href', '/lessons/new')

    const reviewLink = screen.getByRole('link', { name: /review/i })
    expect(reviewLink).toHaveAttribute('href', '/lessons/review')
  })

  it('renders error when fetching lessons count fails with an Error instance', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockRejectedValue(new Error('boom'))
    svc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    svc.getWritingReviewsCount.mockResolvedValue({ count: 0 })

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    expect(await screen.findByText(/error: boom/i)).toBeInTheDocument()
  })

  it('renders fallback error message when fetching lessons count fails with a non-Error value', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockRejectedValue('string error')
    svc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    svc.getWritingReviewsCount.mockResolvedValue({ count: 0 })

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    expect(await screen.findByText(/error: failed to load lesson count/i)).toBeInTheDocument()
  })

  it('renders error when fetching reviews count fails with an Error instance', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockResolvedValue({ count: 0 })
    svc.getLessonReviewsCount.mockRejectedValue(new Error('review error'))
    svc.getWritingReviewsCount.mockResolvedValue({ count: 0 })

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    expect(await screen.findByText(/error: review error/i)).toBeInTheDocument()
  })

  it('renders fallback error message when fetching reviews count fails with a non-Error value', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockResolvedValue({ count: 0 })
    svc.getLessonReviewsCount.mockRejectedValue('string error')
    svc.getWritingReviewsCount.mockResolvedValue({ count: 0 })

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    expect(await screen.findByText(/error: failed to load review count/i)).toBeInTheDocument()
  })

  it('renders error when fetching writing count fails with an Error instance', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockResolvedValue({ count: 0 })
    svc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    svc.getWritingReviewsCount.mockRejectedValue(new Error('writing error'))

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    expect(await screen.findByText(/error: writing error/i)).toBeInTheDocument()
  })

  it('renders fallback error message when fetching writing count fails with a non-Error value', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockResolvedValue({ count: 0 })
    svc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    svc.getWritingReviewsCount.mockRejectedValue('string error')

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    expect(await screen.findByText(/error: failed to load writing review count/i)).toBeInTheDocument()
  })

  it('renders disabled divs (not links) for all cards when all counts are 0 after loading', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockResolvedValue({ count: 0 })
    svc.getLessonReviewsCount.mockResolvedValue({ count: 0 })
    svc.getWritingReviewsCount.mockResolvedValue({ count: 0 })

    const { waitFor } = await import('@testing-library/react')

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    // After loaded=true with count=0, NavLinks become divs — wait for links to disappear
    await waitFor(() => {
      expect(screen.queryByRole('link', { name: /learn/i })).not.toBeInTheDocument()
      expect(screen.queryByRole('link', { name: /review/i })).not.toBeInTheDocument()
      expect(screen.queryByRole('link', { name: /writing/i })).not.toBeInTheDocument()
    })

    // Headings should still be visible
    expect(screen.getByRole('heading', { name: /learn/i })).toBeInTheDocument()
  })

  it('renders singular lesson label when count is 1', async () => {
    const svc = lessonService as unknown as { getLessonsCount: ReturnType<typeof vi.fn>, getLessonReviewsCount: ReturnType<typeof vi.fn>, getWritingReviewsCount: ReturnType<typeof vi.fn> }
    svc.getLessonsCount.mockResolvedValue({ count: 1 })
    svc.getLessonReviewsCount.mockResolvedValue({ count: 1 })
    svc.getWritingReviewsCount.mockResolvedValue({ count: 1 })

    render(
      <MemoryRouter>
        <LessonsPage />
      </MemoryRouter>
    )

    expect(await screen.findByText('new lesson available')).toBeInTheDocument()
    expect(screen.getByText('item to review')).toBeInTheDocument()
    expect(screen.getByText('item to practice')).toBeInTheDocument()
  })
})
