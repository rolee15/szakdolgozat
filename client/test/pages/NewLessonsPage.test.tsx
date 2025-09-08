import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import NewLessonsPage from '@/pages/NewLessonsPage'

const navigateSpy = vi.fn()
vi.mock('react-router-dom', async (orig) => {
  const actual = await orig()
  return {
    ...actual,
    useNavigate: () => navigateSpy,
  }
})

vi.mock('@/services/lessonService', () => ({
  default: {
    getLessons: vi.fn(),
    postLearnLesson: vi.fn(),
  }
}))

import lessonService from '@/services/lessonService'

describe('NewLessonsPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })
  afterEach(() => {
    vi.clearAllMocks()
  })

  it('loads lessons, shows current, and navigates after last Next', async () => {
    navigateSpy.mockClear()

    const svc = lessonService as unknown as { getLessons: ReturnType<typeof vi.fn>, postLearnLesson: ReturnType<typeof vi.fn> }
    svc.getLessons.mockResolvedValue([
      { characterId: 1, symbol: 'あ', romanization: 'a', type: 0 },
      { characterId: 2, symbol: 'い', romanization: 'i', type: 0 },
    ])
    svc.postLearnLesson.mockResolvedValue(undefined)

    render(<NewLessonsPage />)

    // First symbol shows up
    expect(await screen.findByText('あ')).toBeInTheDocument()

    // Click Next twice
    const nextBtn = screen.getByRole('button', { name: /next/i })
    fireEvent.click(nextBtn)
    expect(await screen.findByText('い')).toBeInTheDocument()
    fireEvent.click(nextBtn)

    // After last lesson, navigates to /lessons (async state)
    await import('@testing-library/react').then(({ waitFor }) => waitFor(() => {
      expect(navigateSpy).toHaveBeenCalledWith('/lessons')
    }))
  })
})
