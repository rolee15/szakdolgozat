import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/routes', () => ({
  API_USERS_URL: 'http://api.test/users',
  API_USERS_SETTINGS_URL: 'http://api.test/users/settings',
  API_USERS_FORGOT_PASSWORD_URL: 'http://api.test/users/forgot-password',
  API_USERS_RESET_PASSWORD_URL: 'http://api.test/users/reset-password',
  API_USERS_ACTIVATE_URL: 'http://api.test/users/activate',
}));

import userService from '@/services/userService';

function mockFetchOk(data: unknown) {
  const json = vi.fn().mockResolvedValue(data);
  const okResponse = { ok: true, json } as unknown as Response;
  (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(okResponse) as unknown as typeof fetch;
}

function mockFetchNotOk() {
  const notOkResponse = { ok: false, json: vi.fn() } as unknown as Response;
  (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(notOkResponse) as unknown as typeof fetch;
}

describe('userService', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  describe('login', () => {
    it('posts to /login with body and returns JSON', async () => {
      const payload = { token: 'jwt', userId: 1 };
      mockFetchOk(payload);

      const email = 'test@example.com';
      const password = 'secret';
      const result = await userService.login(email, password);

      expect(result).toEqual(payload);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/login');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
      expect(init?.body).toBe(JSON.stringify({ email, password }));
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(userService.login('a@b.c', 'x')).rejects.toThrow('Failed to login');
    });
  });

  describe('register', () => {
    it('posts to /register with body and returns JSON', async () => {
      const payload = { id: 1, email: 'new@example.com' };
      mockFetchOk(payload);

      const email = 'new@example.com';
      const password = 'secret';
      const result = await userService.register(email, password);

      expect(result).toEqual(payload);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/register');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
      expect(init?.body).toBe(JSON.stringify({ email, password }));
    });

    it('throws on non-ok response when json body is unavailable', async () => {
      const notOkResponse = {
        ok: false,
        json: vi.fn().mockRejectedValue(new Error('no json')),
      } as unknown as Response;
      (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(notOkResponse) as unknown as typeof fetch;

      await expect(userService.register('x@y.z', 'p')).rejects.toThrow('Failed to register');
    });

    it('throws backend error message on non-ok response', async () => {
      const notOkResponse = {
        ok: false,
        json: vi.fn().mockResolvedValue({ errorMessage: 'Username already exists' }),
      } as unknown as Response;
      (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(notOkResponse) as unknown as typeof fetch;

      await expect(userService.register('x@y.z', 'p')).rejects.toThrow('Username already exists');
    });
  });

  describe('refreshToken', () => {
    it('posts to /refreshToken with both token and refreshToken', async () => {
      const payload = { token: 'newjwt' };
      mockFetchOk(payload);

      const token = 'oldjwt';
      const refreshToken = 'myrefreshtoken';
      const result = await userService.refreshToken(token, refreshToken);

      expect(result).toEqual(payload);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/refreshToken');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
      expect(init?.body).toBe(JSON.stringify({ token, refreshToken }));
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(userService.refreshToken('badtoken', 'refreshtoken')).rejects.toThrow('Failed to refresh token');
    });
  });

  describe('getSettings', () => {
    it('fetches settings from the correct endpoint and returns JSON', async () => {
      const payload = { dailyLessonLimit: 10, reviewBatchSize: 50, jlptLevel: 'N5' };
      mockFetchOk(payload);

      const result = await userService.getSettings();

      expect(result).toEqual(payload);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/settings');
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(userService.getSettings()).rejects.toThrow('Failed to load settings');
    });
  });

  describe('updateSettings', () => {
    it('sends PUT with correct body and resolves on success', async () => {
      const notOkResponse = { ok: true, json: vi.fn() } as unknown as Response;
      (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(notOkResponse) as unknown as typeof fetch;

      const dto = { dailyLessonLimit: 20, reviewBatchSize: 100, jlptLevel: 'N4' };
      await userService.updateSettings(dto);

      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/settings');
      expect(init?.method).toBe('PUT');
      expect(init?.body).toBe(JSON.stringify(dto));
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      const dto = { dailyLessonLimit: 5, reviewBatchSize: 20, jlptLevel: 'N3' };
      await expect(userService.updateSettings(dto)).rejects.toThrow('Failed to save settings');
    });
  });

  describe('forgotPassword', () => {
    it('posts to /forgot-password with email and resolves on success', async () => {
      const okResponse = { ok: true, json: vi.fn() } as unknown as Response;
      (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(okResponse) as unknown as typeof fetch;

      const email = 'user@example.com';
      await userService.forgotPassword(email);

      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/forgot-password');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
      expect(init?.body).toBe(JSON.stringify({ email }));
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(userService.forgotPassword('user@example.com')).rejects.toThrow('Failed to send reset code');
    });
  });

  describe('confirmResetPassword', () => {
    it('posts to /reset-password with email, code, and newPassword and resolves on success', async () => {
      const okResponse = { ok: true, json: vi.fn() } as unknown as Response;
      (globalThis as { fetch: typeof fetch }).fetch = vi.fn().mockResolvedValue(okResponse) as unknown as typeof fetch;

      const email = 'user@example.com';
      const code = '123456';
      const newPassword = 'newpass123';
      await userService.confirmResetPassword(email, code, newPassword);

      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/reset-password');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
      expect(init?.body).toBe(JSON.stringify({ email, resetCode: code, newPassword }));
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(
        userService.confirmResetPassword('user@example.com', 'badcode', 'newpass123')
      ).rejects.toThrow('Invalid or expired reset code');
    });
  });

  describe('activateAccount', () => {
    it('calls correct URL with encoded token', async () => {
      const payload = { success: true, message: 'Account activated.' };
      mockFetchOk(payload);

      const token = 'test token/with special+chars';
      await userService.activateAccount(token);

      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe(`http://api.test/users/activate?token=${encodeURIComponent(token)}`);
      expect(init?.method).toBe('POST');
    });

    it('returns parsed JSON response', async () => {
      const payload = { success: true, message: 'Account activated.' };
      mockFetchOk(payload);

      const result = await userService.activateAccount('sometoken');

      expect(result).toEqual(payload);
    });
  });
});
