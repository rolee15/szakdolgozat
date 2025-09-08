import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import HiraganaPage from '@/pages/HiraganaPage'

vi.mock('@/components/common/KanaGrid', () => ({ default: ({ type }: { type: string }) => <div>Grid:{type}</div> }))

describe('HiraganaPage', () => {
  it('renders KanaGrid with type hiragana', () => {
    render(<HiraganaPage />)
    expect(screen.getByText('Grid:hiragana')).toBeInTheDocument()
  })
})
