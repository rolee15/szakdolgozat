import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import ReviewLessonsPage from '@/pages/ReviewLessonsPage'

vi.mock('@/components/lessons/LessonReviewInput', () => ({
  default: ({ onSubmit, buttonClassName }: { onSubmit: (a: string) => void; buttonClassName?: string }) => (
    <button data-testid="reading-input" data-button-class={buttonClassName} onClick={() => onSubmit('ans')}>
      ReadingMock
    </button>
  ),
}))

vi.mock('@/components/lessons/WritingInput', () => ({
  default: ({ onSubmit, buttonClassName }: { onSubmit: (a: string) => void; buttonClassName?: string }) => (
    <button data-testid="writing-input" data-button-class={buttonClassName} onClick={() => onSubmit('ans')}>
      WritingMock
    </button>
  ),
}))

vi.mock('@/services/lessonService', () => ({
  default: {
    getLessonReviews: vi.fn(),
    getWritingReviews: vi.fn(),
    postLessonReviewCheck: vi.fn(),
    postWritingReviewCheck: vi.fn(),
  },
}))

import lessonService from '@/services/lessonService'

type SvcMocks = {
  getLessonReviews: ReturnType<typeof vi.fn>
  getWritingReviews: ReturnType<typeof vi.fn>
  postLessonReviewCheck: ReturnType<typeof vi.fn>
  postWritingReviewCheck: ReturnType<typeof vi.fn>
}

const svc = lessonService as unknown as SvcMocks

describe('ReviewLessonsPage (merged reading + writing)', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })
  afterEach(() => {
    vi.clearAllMocks()
  })

  it('shows no items message when both queues are empty', async () => {
    svc.getLessonReviews.mockResolvedValue([])
    svc.getWritingReviews.mockResolvedValue([])

    render(<ReviewLessonsPage />)

    expect(await screen.findByText(/no more items to review/i)).toBeInTheDocument()
  })

  it('renders the reading input with reading-coloured button for a reading question', async () => {
    svc.getLessonReviews.mockResolvedValue([{ question: 'あ' }])
    svc.getWritingReviews.mockResolvedValue([])

    render(<ReviewLessonsPage />)

    expect(await screen.findByText('あ')).toBeInTheDocument()
    expect(await screen.findByText('Reading')).toBeInTheDocument()

    const button = await screen.findByTestId('reading-input')
    expect(button.getAttribute('data-button-class')).toMatch(/purple/)
  })

  it('renders the writing input with writing-coloured button for a writing question', async () => {
    svc.getLessonReviews.mockResolvedValue([])
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])

    render(<ReviewLessonsPage />)

    expect(await screen.findByText('a')).toBeInTheDocument()
    expect(await screen.findByText('Writing · Hiragana')).toBeInTheDocument()

    const button = await screen.findByTestId('writing-input')
    expect(button.getAttribute('data-button-class')).toMatch(/orange/)
  })

  it('interleaves reading and writing items so types alternate', async () => {
    svc.getLessonReviews.mockResolvedValue([{ question: 'あ' }, { question: 'い' }])
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })
    svc.postWritingReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'あ' })

    render(<ReviewLessonsPage />)

    // Item 1: first reading question
    expect(await screen.findByText('あ')).toBeInTheDocument()
    fireEvent.click(screen.getByTestId('reading-input'))
    fireEvent.click(await screen.findByRole('button', { name: /continue/i }))

    // Item 2: writing question (interleaved)
    expect(await screen.findByText('a')).toBeInTheDocument()
    fireEvent.click(screen.getByTestId('writing-input'))
    fireEvent.click(await screen.findByRole('button', { name: /continue/i }))

    // Item 3: second reading question
    expect(await screen.findByText('い')).toBeInTheDocument()
  })

  it('submits a reading answer and shows correct feedback', async () => {
    svc.getLessonReviews.mockResolvedValue([{ question: 'あ' }])
    svc.getWritingReviews.mockResolvedValue([])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })

    render(<ReviewLessonsPage />)

    fireEvent.click(await screen.findByTestId('reading-input'))

    expect(await screen.findByText(/correct!/i)).toBeInTheDocument()
    expect(svc.postLessonReviewCheck).toHaveBeenCalledWith('あ', 'ans')
  })

  it('submits a writing answer and shows correct feedback', async () => {
    svc.getLessonReviews.mockResolvedValue([])
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 7, romanization: 'a', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'あ' })

    render(<ReviewLessonsPage />)

    fireEvent.click(await screen.findByTestId('writing-input'))

    expect(await screen.findByText(/correct!/i)).toBeInTheDocument()
    expect(svc.postWritingReviewCheck).toHaveBeenCalledWith(7, 'ans')
  })

  it('shows incorrect feedback with correct answer when wrong', async () => {
    svc.getLessonReviews.mockResolvedValue([{ question: 'あ' }])
    svc.getWritingReviews.mockResolvedValue([])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: false, correctAnswer: 'a' })

    render(<ReviewLessonsPage />)

    fireEvent.click(await screen.findByTestId('reading-input'))

    expect(await screen.findByText(/incorrect/i)).toBeInTheDocument()
    expect(await screen.findByText('a')).toBeInTheDocument()
  })

  it('cycles incorrect items to end of queue so the session is not complete', async () => {
    svc.getLessonReviews.mockResolvedValue([{ question: 'あ' }])
    svc.getWritingReviews.mockResolvedValue([])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: false, correctAnswer: 'a' })

    render(<ReviewLessonsPage />)

    fireEvent.click(await screen.findByTestId('reading-input'))
    fireEvent.click(await screen.findByRole('button', { name: /continue/i }))

    // The same prompt is still visible — the item cycled to the end of the queue
    expect(await screen.findByText('あ')).toBeInTheDocument()
    expect(screen.queryByText(/no more items to review/i)).not.toBeInTheDocument()
  })

  it('removes correctly-answered items from the queue', async () => {
    svc.getLessonReviews.mockResolvedValue([{ question: 'あ' }])
    svc.getWritingReviews.mockResolvedValue([])
    svc.postLessonReviewCheck.mockResolvedValue({ isCorrect: true, correctAnswer: 'a' })

    render(<ReviewLessonsPage />)

    fireEvent.click(await screen.findByTestId('reading-input'))
    fireEvent.click(await screen.findByRole('button', { name: /continue/i }))

    expect(await screen.findByText(/no more items to review/i)).toBeInTheDocument()
  })

  it('shows error when fetching fails', async () => {
    svc.getLessonReviews.mockRejectedValue(new Error('Network failure'))
    svc.getWritingReviews.mockResolvedValue([])

    render(<ReviewLessonsPage />)

    expect(await screen.findByText(/error: network failure/i)).toBeInTheDocument()
  })

  it('uses fallback message when fetch rejects with non-Error value', async () => {
    svc.getLessonReviews.mockRejectedValue('boom')
    svc.getWritingReviews.mockResolvedValue([])

    render(<ReviewLessonsPage />)

    expect(await screen.findByText(/failed to load reviews/i)).toBeInTheDocument()
  })

  it('shows error when reading check fails', async () => {
    svc.getLessonReviews.mockResolvedValue([{ question: 'あ' }])
    svc.getWritingReviews.mockResolvedValue([])
    svc.postLessonReviewCheck.mockRejectedValue(new Error('Check failed'))

    render(<ReviewLessonsPage />)

    fireEvent.click(await screen.findByTestId('reading-input'))

    expect(await screen.findByText(/error: check failed/i)).toBeInTheDocument()
  })

  it('shows error when writing check fails', async () => {
    svc.getLessonReviews.mockResolvedValue([])
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 1, romanization: 'a', characterType: 'hiragana' },
    ])
    svc.postWritingReviewCheck.mockRejectedValue(new Error('Writing check failed'))

    render(<ReviewLessonsPage />)

    fireEvent.click(await screen.findByTestId('writing-input'))

    expect(await screen.findByText(/error: writing check failed/i)).toBeInTheDocument()
  })

  it('uses fallback message when check rejects with non-Error value', async () => {
    svc.getLessonReviews.mockResolvedValue([{ question: 'あ' }])
    svc.getWritingReviews.mockResolvedValue([])
    svc.postLessonReviewCheck.mockRejectedValue('boom')

    render(<ReviewLessonsPage />)

    fireEvent.click(await screen.findByTestId('reading-input'))

    expect(await screen.findByText(/failed to check answer/i)).toBeInTheDocument()
  })

  it('shows the writing-katakana label for katakana writing items', async () => {
    svc.getLessonReviews.mockResolvedValue([])
    svc.getWritingReviews.mockResolvedValue([
      { characterId: 3, romanization: 'a', characterType: 'katakana' },
    ])

    render(<ReviewLessonsPage />)

    expect(await screen.findByText('Writing · Katakana')).toBeInTheDocument()
  })
})
