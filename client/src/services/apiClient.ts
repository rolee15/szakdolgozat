import { API_USERS_URL } from '@/services/routes';

const getToken = () => localStorage.getItem('token');
const getRefreshToken = () => localStorage.getItem('refreshToken');

let isRefreshing = false;
let refreshQueue: Array<(token: string | null) => void> = [];

async function attemptTokenRefresh(): Promise<string | null> {
  const token = getToken();
  const refreshToken = getRefreshToken();
  if (!token || !refreshToken) return null;

  const response = await fetch(`${API_USERS_URL}/refreshToken`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ token, refreshToken }),
  });

  if (!response.ok) return null;

  const dto = await response.json();
  if (!dto.token) return null;

  localStorage.setItem('token', dto.token);
  return dto.token as string;
}

function buildHeaders(options: RequestInit): Record<string, string> {
  const token = getToken();
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...(options.headers as Record<string, string> ?? {}),
  };
  if (token) headers['Authorization'] = `Bearer ${token}`;
  return headers;
}

export async function apiFetch(url: string, options: RequestInit = {}): Promise<Response> {
  const response = await fetch(url, { ...options, headers: buildHeaders(options) });

  if (response.status !== 401) return response;

  if (isRefreshing) {
    return new Promise<Response>(resolve => {
      refreshQueue.push(async (newToken) => {
        if (!newToken) {
          resolve(response);
          return;
        }
        const retryHeaders = { ...buildHeaders(options), Authorization: `Bearer ${newToken}` };
        resolve(await fetch(url, { ...options, headers: retryHeaders }));
      });
    });
  }

  isRefreshing = true;
  const newToken = await attemptTokenRefresh().catch(() => null);
  isRefreshing = false;

  const queued = refreshQueue.splice(0);
  queued.forEach(cb => cb(newToken));

  if (!newToken) {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    window.location.href = '/login';
    return response;
  }

  const retryHeaders = { ...buildHeaders(options), Authorization: `Bearer ${newToken}` };
  return fetch(url, { ...options, headers: retryHeaders });
}
