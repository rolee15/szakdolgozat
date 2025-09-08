import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';

vi.mock('@/services/routes', () => ({
  API_USERS_URL: 'http://api.test/users',
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

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(userService.register('x@y.z', 'p')).rejects.toThrow('Failed to register');
    });
  });

  describe('resetPassword', () => {
    it('posts to /resetPassword with body and returns JSON', async () => {
      const payload = { success: true };
      mockFetchOk(payload);

      const email = 'reset@example.com';
      const result = await userService.resetPassword(email);

      expect(result).toEqual(payload);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/resetPassword');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
      expect(init?.body).toBe(JSON.stringify({ email }));
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(userService.resetPassword('a@b.c')).rejects.toThrow('Failed to send reset password email');
    });
  });

  describe('refreshToken', () => {
    it('posts to /refreshToken with body and returns JSON', async () => {
      const payload = { token: 'newjwt' };
      mockFetchOk(payload);

      const token = 'oldjwt';
      const result = await userService.refreshToken(token);

      expect(result).toEqual(payload);
      const fetchMock = global.fetch as unknown as ReturnType<typeof vi.fn>;
      const [url, init] = fetchMock.mock.calls[0] as [string, RequestInit];
      expect(url).toBe('http://api.test/users/refreshToken');
      expect(init?.method).toBe('POST');
      expect(init?.headers).toEqual({ 'Content-Type': 'application/json' });
      expect(init?.body).toBe(JSON.stringify({ token }));
    });

    it('throws on non-ok response', async () => {
      mockFetchNotOk();
      await expect(userService.refreshToken('badtoken')).rejects.toThrow('Failed to refresh token');
    });
  });
});
