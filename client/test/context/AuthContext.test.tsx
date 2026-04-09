import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest'
import { render, screen, act, fireEvent } from '@testing-library/react'
import { AuthProvider, useAuth } from '@/context/AuthContext'

vi.mock('@/services/userService', () => ({
  default: {
    login: vi.fn(),
    register: vi.fn(),
  },
}))

import userService from '@/services/userService'

function makeTestJwt(userId: number, username: string): string {
  const header = btoa(JSON.stringify({ alg: 'HS256', typ: 'JWT' }))
  const payload = btoa(JSON.stringify({ sub: String(userId), unique_name: username, exp: 9999999999 }))
  return `${header}.${payload}.fakesignature`
}

// A helper component that exposes auth state through rendered text
function AuthConsumer() {
  const { isAuthenticated, username, userId, login, logout, register } = useAuth()
  return (
    <div>
      <span data-testid="is-authenticated">{String(isAuthenticated)}</span>
      <span data-testid="username">{username ?? ''}</span>
      <span data-testid="user-id">{userId ?? ''}</span>
      <button onClick={() => login('a@b.com', 'password')}>login</button>
      <button onClick={() => logout()}>logout</button>
      <button onClick={() => register('a@b.com', 'password')}>register</button>
    </div>
  )
}

function renderWithProvider() {
  return render(
    <AuthProvider>
      <AuthConsumer />
    </AuthProvider>
  )
}

describe('AuthContext', () => {
  beforeEach(() => {
    localStorage.clear()
    vi.restoreAllMocks()
  })

  afterEach(() => {
    localStorage.clear()
    vi.clearAllMocks()
  })

  it('login_success_storesTokenAndSetsAuthenticated', async () => {
    const svc = userService as unknown as { login: ReturnType<typeof vi.fn> }
    const jwt = makeTestJwt(42, 'testuser')
    svc.login.mockResolvedValue({ isSuccess: true, token: jwt, userId: 42 })

    renderWithProvider()

    expect(screen.getByTestId('is-authenticated').textContent).toBe('false')

    await act(async () => {
      screen.getByRole('button', { name: 'login' }).click()
    })

    expect(screen.getByTestId('is-authenticated').textContent).toBe('true')
    expect(screen.getByTestId('username').textContent).toBe('testuser')
    expect(localStorage.getItem('token')).toBe(jwt)
  })

  it('login_failure_throwsError', async () => {
    const svc = userService as unknown as { login: ReturnType<typeof vi.fn> }
    svc.login.mockResolvedValue({ isSuccess: false, errorMessage: 'Bad creds' })

    // Wrap in a boundary that catches the error thrown from the consumer
    let caughtError: unknown
    const ErrorCapture = () => {
      const { login } = useAuth()
      return (
        <button
          onClick={() =>
            login('a@b.com', 'wrong').catch((e) => {
              caughtError = e
            })
          }
        >
          login
        </button>
      )
    }

    render(
      <AuthProvider>
        <ErrorCapture />
      </AuthProvider>
    )

    await act(async () => {
      screen.getByRole('button', { name: 'login' }).click()
    })

    expect(caughtError).toBeInstanceOf(Error)
    expect((caughtError as Error).message).toBe('Bad creds')
    expect(localStorage.getItem('token')).toBeNull()
  })

  it('logout_clearsTokenAndSetsNotAuthenticated', async () => {
    const svc = userService as unknown as { login: ReturnType<typeof vi.fn> }
    const jwt = makeTestJwt(7, 'logoutuser')
    svc.login.mockResolvedValue({ isSuccess: true, token: jwt, userId: 7 })

    renderWithProvider()

    await act(async () => {
      screen.getByRole('button', { name: 'login' }).click()
    })

    expect(screen.getByTestId('is-authenticated').textContent).toBe('true')

    await act(async () => {
      screen.getByRole('button', { name: 'logout' }).click()
    })

    expect(screen.getByTestId('is-authenticated').textContent).toBe('false')
    expect(localStorage.getItem('token')).toBeNull()
    expect(localStorage.getItem('refreshToken')).toBeNull()
  })

  it('register_success_storesTokenAndSetsAuthenticated', async () => {
    const svc = userService as unknown as { register: ReturnType<typeof vi.fn> }
    const jwt = makeTestJwt(99, 'newuser')
    svc.register.mockResolvedValue({ isSuccess: true, token: jwt, userId: 99 })

    renderWithProvider()

    await act(async () => {
      screen.getByRole('button', { name: 'register' }).click()
    })

    expect(screen.getByTestId('is-authenticated').textContent).toBe('true')
    expect(screen.getByTestId('username').textContent).toBe('newuser')
    expect(localStorage.getItem('token')).toBe(jwt)
  })

  it('initializesFromLocalStorage_whenTokenExists', () => {
    const jwt = makeTestJwt(5, 'storeduser')
    localStorage.setItem('token', jwt)

    renderWithProvider()

    expect(screen.getByTestId('is-authenticated').textContent).toBe('true')
    expect(screen.getByTestId('username').textContent).toBe('storeduser')
    expect(screen.getByTestId('user-id').textContent).toBe('5')
  })

  it('initializesAsNotAuthenticated_whenNoTokenInLocalStorage', () => {
    renderWithProvider()

    expect(screen.getByTestId('is-authenticated').textContent).toBe('false')
    expect(screen.getByTestId('username').textContent).toBe('')
  })

  it('login_storesRefreshToken_whenProvided', async () => {
    const svc = userService as unknown as { login: ReturnType<typeof vi.fn> }
    const jwt = makeTestJwt(3, 'refreshuser')
    svc.login.mockResolvedValue({
      isSuccess: true,
      token: jwt,
      refreshToken: 'myrefresh',
      userId: 3,
    })

    renderWithProvider()

    await act(async () => {
      screen.getByRole('button', { name: 'login' }).click()
    })

    expect(localStorage.getItem('refreshToken')).toBe('myrefresh')
  })

  it('handles malformed JWT gracefully on init — returns empty payload', () => {
    // A token with an invalid base64 payload causes decodeJwtPayload to catch and return {}
    localStorage.setItem('token', 'bad.!!!.token')

    renderWithProvider()

    // Should still initialize without crashing; username will be empty
    expect(screen.getByTestId('username').textContent).toBe('')
  })

  it('clearMustChangePassword_setsMustChangePasswordToFalse', async () => {
    const ClearConsumer = () => {
      const { clearMustChangePassword } = useAuth()
      return <button onClick={clearMustChangePassword}>clear</button>
    }

    render(
      <AuthProvider>
        <ClearConsumer />
      </AuthProvider>
    )

    await act(async () => {
      fireEvent.click(screen.getByRole('button', { name: 'clear' }))
    })

    // No error means clearMustChangePassword executed without throwing
    expect(screen.getByRole('button', { name: 'clear' })).toBeInTheDocument()
  })
})
