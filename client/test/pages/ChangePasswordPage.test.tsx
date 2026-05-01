import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import ChangePasswordPage from '@/pages/ChangePasswordPage'

const mockClearMustChangePassword = vi.fn()
const mockUseAuth = vi.fn()
vi.mock('@/context/AuthContext', () => ({
  useAuth: () => mockUseAuth(),
}))

const navigateSpy = vi.fn()
vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual<typeof import('react-router-dom')>('react-router-dom')
  return {
    ...actual,
    useNavigate: () => navigateSpy,
  }
})

vi.mock('@/services/userService', () => ({
  default: {
    changePassword: vi.fn(),
  },
}))

import userService from '@/services/userService'

function renderPage(mustChangePassword = false) {
  mockUseAuth.mockReturnValue({
    mustChangePassword,
    clearMustChangePassword: mockClearMustChangePassword,
  })
  return render(
    <MemoryRouter>
      <ChangePasswordPage />
    </MemoryRouter>
  )
}

describe('ChangePasswordPage', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
    navigateSpy.mockClear()
    mockClearMustChangePassword.mockClear()
  })

  afterEach(() => {
    vi.clearAllMocks()
  })

  it('renders current password, new password, and confirm password fields', () => {
    renderPage()

    expect(screen.getByLabelText(/current password/i)).toBeInTheDocument()
    expect(screen.getByLabelText(/^new password$/i)).toBeInTheDocument()
    expect(screen.getByLabelText(/confirm new password/i)).toBeInTheDocument()
    expect(screen.getByRole('button', { name: /change password/i })).toBeInTheDocument()
  })

  it('shows must-change-password warning when mustChangePassword is true', () => {
    renderPage(true)

    expect(screen.getByText(/you must change your password before continuing/i)).toBeInTheDocument()
  })

  it('does not show warning when mustChangePassword is false', () => {
    renderPage(false)

    expect(screen.queryByText(/you must change your password before continuing/i)).not.toBeInTheDocument()
  })

  it('shows validation error when new password is too short', async () => {
    renderPage()

    fireEvent.change(screen.getByLabelText(/current password/i), { target: { value: 'oldpass123' } })
    fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'short' } })
    fireEvent.change(screen.getByLabelText(/confirm new password/i), { target: { value: 'short' } })
    fireEvent.submit(screen.getByRole('button', { name: /change password/i }).closest('form') as HTMLFormElement)

    expect(await screen.findByRole('alert')).toHaveTextContent(/at least 8 characters/i)
  })

  it('shows validation error when passwords do not match', async () => {
    renderPage()

    fireEvent.change(screen.getByLabelText(/current password/i), { target: { value: 'oldpass123' } })
    fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpassword1' } })
    fireEvent.change(screen.getByLabelText(/confirm new password/i), { target: { value: 'differentpass' } })
    fireEvent.submit(screen.getByRole('button', { name: /change password/i }).closest('form') as HTMLFormElement)

    expect(await screen.findByRole('alert')).toHaveTextContent(/passwords do not match/i)
  })

  it('calls userService.changePassword and navigates on successful submission', async () => {
    const svc = userService as unknown as { changePassword: ReturnType<typeof vi.fn> }
    svc.changePassword.mockResolvedValue({ isSuccess: true })

    renderPage()

    fireEvent.change(screen.getByLabelText(/current password/i), { target: { value: 'oldpass123' } })
    fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpassword1' } })
    fireEvent.change(screen.getByLabelText(/confirm new password/i), { target: { value: 'newpassword1' } })
    fireEvent.submit(screen.getByRole('button', { name: /change password/i }).closest('form') as HTMLFormElement)

    await vi.waitFor(() => {
      expect(svc.changePassword).toHaveBeenCalledWith('oldpass123', 'newpassword1')
      expect(mockClearMustChangePassword).toHaveBeenCalled()
      expect(navigateSpy).toHaveBeenCalledWith('/lessons', { replace: true })
    })
  })

  it('shows error message on failed submission', async () => {
    const svc = userService as unknown as { changePassword: ReturnType<typeof vi.fn> }
    svc.changePassword.mockRejectedValue(new Error('Incorrect current password'))

    renderPage()

    fireEvent.change(screen.getByLabelText(/current password/i), { target: { value: 'wrongpass' } })
    fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpassword1' } })
    fireEvent.change(screen.getByLabelText(/confirm new password/i), { target: { value: 'newpassword1' } })
    fireEvent.submit(screen.getByRole('button', { name: /change password/i }).closest('form') as HTMLFormElement)

    expect(await screen.findByRole('alert')).toHaveTextContent('Incorrect current password')
  })

  it('shows generic error message when error has no message', async () => {
    const svc = userService as unknown as { changePassword: ReturnType<typeof vi.fn> }
    svc.changePassword.mockRejectedValue('unknown error')

    renderPage()

    fireEvent.change(screen.getByLabelText(/current password/i), { target: { value: 'oldpass123' } })
    fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpassword1' } })
    fireEvent.change(screen.getByLabelText(/confirm new password/i), { target: { value: 'newpassword1' } })
    fireEvent.submit(screen.getByRole('button', { name: /change password/i }).closest('form') as HTMLFormElement)

    expect(await screen.findByRole('alert')).toHaveTextContent(/failed to change password/i)
  })

  it('disables the button while submitting', async () => {
    const svc = userService as unknown as { changePassword: ReturnType<typeof vi.fn> }
    // Never resolves so button stays disabled
    svc.changePassword.mockReturnValue(new Promise(() => {}))

    renderPage()

    fireEvent.change(screen.getByLabelText(/current password/i), { target: { value: 'oldpass123' } })
    fireEvent.change(screen.getByLabelText(/^new password$/i), { target: { value: 'newpassword1' } })
    fireEvent.change(screen.getByLabelText(/confirm new password/i), { target: { value: 'newpassword1' } })
    fireEvent.submit(screen.getByRole('button').closest('form') as HTMLFormElement)

    await vi.waitFor(() => {
      expect(screen.getByRole('button', { name: /changing\.\.\./i })).toBeDisabled()
    })
  })
})
