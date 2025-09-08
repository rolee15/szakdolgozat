import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import KatakanaPage from '@/pages/KatakanaPage'

vi.mock('@/components/common/KanaGrid', () => ({ default: ({ type }: { type: string }) => <div>Grid:{type}</div> }))

describe('KatakanaPage', () => {
  it('renders KanaGrid with type katakana', () => {
    render(<KatakanaPage />)
    expect(screen.getByText('Grid:katakana')).toBeInTheDocument()
  })
})
