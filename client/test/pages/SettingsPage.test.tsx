import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent, waitFor } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

vi.mock('@/services/userService', () => ({
  default: {
    getSettings: vi.fn(),
    updateSettings: vi.fn(),
  },
}))

import userService from '@/services/userService'

const svc = userService as unknown as {
  getSettings: ReturnType<typeof vi.fn>
  updateSettings: ReturnType<typeof vi.fn>
}

const defaultSettings: UserSettings = {
  dailyLessonLimit: 10,
  reviewBatchSize: 50,
  jlptLevel: 'N5',
}

function renderPage() {
  const queryClient = new QueryClient({
    defaultOptions: { queries: { retry: false } },
  })
  return render(
    <QueryClientProvider client={queryClient}>
      <MemoryRouter>
        {/* lazy import avoids circular dep; direct import is fine here */}
        <SettingsPageComponent />
      </MemoryRouter>
    </QueryClientProvider>
  )
}

import SettingsPage from '@/pages/SettingsPage'

// alias so renderPage can reference it after the vi.mock hoisting
const SettingsPageComponent = SettingsPage

describe('SettingsPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
    svc.getSettings.mockResolvedValue(defaultSettings)
    svc.updateSettings.mockResolvedValue(undefined)
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders all three form fields', async () => {
    renderPage()

    expect(await screen.findByLabelText(/daily lesson limit/i)).toBeInTheDocument()
    expect(screen.getByLabelText(/review batch size/i)).toBeInTheDocument()
    expect(screen.getByLabelText(/jlpt level/i)).toBeInTheDocument()
  })

  it('populates form with loaded settings values', async () => {
    renderPage()

    const limitInput = await screen.findByLabelText(/daily lesson limit/i) as HTMLInputElement
    const batchInput = screen.getByLabelText(/review batch size/i) as HTMLInputElement
    const levelSelect = screen.getByLabelText(/jlpt level/i) as HTMLSelectElement

    expect(limitInput.value).toBe('10')
    expect(batchInput.value).toBe('50')
    expect(levelSelect.value).toBe('N5')
  })

  it('shows success message after successful save', async () => {
    renderPage()

    await screen.findByLabelText(/daily lesson limit/i)

    fireEvent.click(screen.getByRole('button', { name: /save settings/i }))

    expect(await screen.findByRole('status')).toHaveTextContent(/settings saved successfully/i)
  })

  it('calls updateSettings with correct values on submit', async () => {
    renderPage()

    const limitInput = await screen.findByLabelText(/daily lesson limit/i)
    fireEvent.change(limitInput, { target: { value: '20' } })

    fireEvent.click(screen.getByRole('button', { name: /save settings/i }))

    await waitFor(() => {
      expect(svc.updateSettings).toHaveBeenCalledWith({
        dailyLessonLimit: 20,
        reviewBatchSize: 50,
        jlptLevel: 'N5',
      })
    })
  })

  it('shows error message when save fails', async () => {
    svc.updateSettings.mockRejectedValue(new Error('Failed to save settings'))

    renderPage()

    await screen.findByLabelText(/daily lesson limit/i)

    fireEvent.click(screen.getByRole('button', { name: /save settings/i }))

    expect(await screen.findByRole('alert')).toHaveTextContent(/failed to save settings/i)
  })

  it('shows loading state while settings are being fetched', () => {
    svc.getSettings.mockReturnValue(new Promise(() => {}))

    renderPage()

    expect(screen.getByText(/loading/i)).toBeInTheDocument()
  })

  it('disables button and shows saving text while mutation is pending', async () => {
    svc.updateSettings.mockReturnValue(new Promise(() => {}))

    renderPage()

    await screen.findByLabelText(/daily lesson limit/i)

    fireEvent.click(screen.getByRole('button', { name: /save settings/i }))

    await waitFor(() => {
      expect(screen.getByRole('button', { name: /saving\.\.\./i })).toBeDisabled()
    })
  })

  it('shows validation error when dailyLessonLimit exceeds maximum', async () => {
    renderPage()

    const limitInput = await screen.findByLabelText(/daily lesson limit/i)
    fireEvent.change(limitInput, { target: { value: '99' } })
    fireEvent.click(screen.getByRole('button', { name: /save settings/i }))

    expect(await screen.findByText(/maximum value is 50/i)).toBeInTheDocument()
  })

  it('shows validation error when reviewBatchSize exceeds maximum', async () => {
    renderPage()

    await screen.findByLabelText(/daily lesson limit/i)

    const batchInput = screen.getByLabelText(/review batch size/i)
    fireEvent.change(batchInput, { target: { value: '999' } })
    fireEvent.click(screen.getByRole('button', { name: /save settings/i }))

    expect(await screen.findByText(/maximum value is 200/i)).toBeInTheDocument()
  })
})
