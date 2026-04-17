import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'

import LoginPage from '@/pages/LoginPage'
import RegisterPage from '@/pages/RegisterPage'
import ForgotPasswordPage from '@/pages/ForgotPasswordPage'

const mockLogin = vi.fn()
const mockRegister = vi.fn()
vi.mock('@/context/AuthContext', () => ({
  useAuth: () => ({
    login: mockLogin,
    register: mockRegister,
    isAuthenticated: false,
    logout: vi.fn(),
  }),
}))

vi.mock('@/services/userService', () => ({
  default: {
    forgotPassword: vi.fn(),
    confirmResetPassword: vi.fn(),
  },
}))

describe('Auth pages', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })
  afterEach(() => {
    vi.clearAllMocks()
  })

  describe('LoginPage', () => {
    it('submits email and password and calls login', async () => {
      mockLogin.mockResolvedValue(undefined)

      render(
        <MemoryRouter>
          <LoginPage />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'a@b.c' } })
      fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'secret' } })
      fireEvent.submit(screen.getByRole('button', { name: /login/i }).closest('form') as HTMLFormElement)

      await vi.waitFor(() => {
        expect(mockLogin).toHaveBeenCalledWith('a@b.c', 'secret')
      })
    })

    it('shows error message on failed login', async () => {
      mockLogin.mockRejectedValue(new Error('Invalid credentials'))

      render(
        <MemoryRouter>
          <LoginPage />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/email/i), { target: { value: 'a@b.c' } })
      fireEvent.change(screen.getByLabelText(/password/i), { target: { value: 'wrong' } })
      fireEvent.submit(screen.getByRole('button', { name: /login/i }).closest('form') as HTMLFormElement)

      expect(await screen.findByRole('alert')).toHaveTextContent('Invalid credentials')
    })
  })

  describe('RegisterPage', () => {
    it('renders a heading', () => {
      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )
      expect(screen.getByRole('heading')).toBeInTheDocument()
    })

    it('shows validation error for short password', async () => {
      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/^email/i), { target: { value: 'a@b.c' } })
      fireEvent.change(screen.getByLabelText(/^password$/i), { target: { value: 'short' } })
      fireEvent.blur(screen.getByLabelText(/^password$/i))

      expect(await screen.findByText(/at least 8 characters/i)).toBeInTheDocument()
    })

    it('shows email validation error when email is empty on blur', async () => {
      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )

      fireEvent.blur(screen.getByLabelText(/^email/i))

      expect(await screen.findByText(/email is required/i)).toBeInTheDocument()
    })

    it('shows invalid email error for bad email format', async () => {
      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/^email/i), { target: { value: 'notanemail' } })
      fireEvent.blur(screen.getByLabelText(/^email/i))

      expect(await screen.findByText(/valid email/i)).toBeInTheDocument()
    })

    it('shows confirm password mismatch error on blur', async () => {
      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/^password$/i), { target: { value: 'password123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'different' } })
      fireEvent.blur(screen.getByLabelText(/confirm password/i))

      expect(await screen.findByText(/passwords do not match/i)).toBeInTheDocument()
    })

    it('shows confirm password required error when empty on blur', async () => {
      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )

      fireEvent.blur(screen.getByLabelText(/confirm password/i))

      expect(await screen.findByText(/please confirm your password/i)).toBeInTheDocument()
    })

    it('shows server error on failed registration', async () => {
      mockRegister.mockRejectedValue(new Error('Email already in use'))

      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/^email/i), { target: { value: 'a@b.c' } })
      fireEvent.change(screen.getByLabelText(/^password$/i), { target: { value: 'password123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'password123' } })
      fireEvent.submit(screen.getByRole('button', { name: /^register$/i }).closest('form') as HTMLFormElement)

      expect(await screen.findByRole('alert')).toHaveTextContent('Email already in use')
    })

    it('shows generic error on non-Error registration failure', async () => {
      mockRegister.mockRejectedValue('unexpected failure')

      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/^email/i), { target: { value: 'a@b.c' } })
      fireEvent.change(screen.getByLabelText(/^password$/i), { target: { value: 'password123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'password123' } })
      fireEvent.submit(screen.getByRole('button', { name: /^register$/i }).closest('form') as HTMLFormElement)

      expect(await screen.findByRole('alert')).toHaveTextContent(/registration failed/i)
    })

    it('shows check-your-email message on successful registration and does not call auth login', async () => {
      mockRegister.mockResolvedValue(undefined)

      render(
        <MemoryRouter>
          <RegisterPage />
        </MemoryRouter>
      )

      fireEvent.change(screen.getByLabelText(/^email/i), { target: { value: 'a@b.c' } })
      fireEvent.change(screen.getByLabelText(/^password$/i), { target: { value: 'password123' } })
      fireEvent.change(screen.getByLabelText(/confirm password/i), { target: { value: 'password123' } })
      fireEvent.submit(screen.getByRole('button', { name: /^register$/i }).closest('form') as HTMLFormElement)

      expect(await screen.findByText(/please check your email to activate/i)).toBeInTheDocument()
      expect(mockLogin).not.toHaveBeenCalled()
    })
  })

  describe('ForgotPasswordPage', () => {
    it('renders email input and send reset code button in step 1', () => {
      render(
        <MemoryRouter>
          <ForgotPasswordPage />
        </MemoryRouter>
      )

      expect(screen.getByLabelText(/email/i)).toBeInTheDocument()
      expect(screen.getByRole('button', { name: /send reset code/i })).toBeInTheDocument()
    })
  })
})
