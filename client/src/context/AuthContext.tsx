import { createContext, useContext, useState, ReactNode } from 'react';
import userService from '@/services/userService';

interface AuthState {
  token: string | null;
  refreshToken: string | null;
  userId: number | null;
  username: string | null;
  role: string | null;
  mustChangePassword: boolean;
}

interface AuthContextType extends AuthState {
  login: (email: string, password: string) => Promise<void>;
  register: (email: string, password: string) => Promise<void>;
  logout: () => void;
  clearMustChangePassword: () => void;
  isAuthenticated: boolean;
  isAdmin: boolean;
}

const AuthContext = createContext<AuthContextType | null>(null);

function decodeJwtPayload(token: string): Record<string, string | undefined> {
  try {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')));
  } catch {
    return {};
  }
}

function isTokenExpired(token: string): boolean {
  const payload = decodeJwtPayload(token);
  const exp = payload['exp'];
  if (!exp) return true;
  return Date.now() / 1000 > parseInt(exp);
}

function extractRole(payload: Record<string, string | undefined>): string | null {
  // ClaimTypes.Role serializes as the full URI or as "role" depending on JWT config
  return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
    ?? payload['role']
    ?? null;
}

const EMPTY_STATE: AuthState = { token: null, refreshToken: null, userId: null, username: null, role: null, mustChangePassword: false };

export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, setState] = useState<AuthState>(() => {
    const token = localStorage.getItem('token');
    const refreshToken = localStorage.getItem('refreshToken');
    if (token && !isTokenExpired(token)) {
      const payload = decodeJwtPayload(token);
      return {
        token,
        refreshToken,
        userId: payload.sub ? parseInt(payload.sub) : null,
        username: payload.unique_name ?? null,
        role: extractRole(payload),
        mustChangePassword: payload['must_change_password'] === 'true',
      };
    }
    // Clear stale token on mount
    if (token) {
      localStorage.removeItem('token');
      localStorage.removeItem('refreshToken');
    }
    return EMPTY_STATE;
  });

  const login = async (email: string, password: string) => {
    const dto = await userService.login(email, password);
    if (!dto.isSuccess || !dto.token) throw new Error(dto.errorMessage ?? 'Login failed');
    const payload = decodeJwtPayload(dto.token);
    const newState: AuthState = {
      token: dto.token,
      refreshToken: dto.refreshToken ?? null,
      userId: dto.userId ?? (payload.sub ? parseInt(payload.sub) : null),
      username: payload.unique_name ?? email,
      role: extractRole(payload),
      mustChangePassword: dto.mustChangePassword ?? false,
    };
    localStorage.setItem('token', dto.token);
    if (dto.refreshToken) localStorage.setItem('refreshToken', dto.refreshToken);
    setState(newState);
  };

  const register = async (email: string, password: string) => {
    const dto = await userService.register(email, password);
    if (!dto.success) throw new Error(dto.message ?? 'Registration failed');
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    setState(EMPTY_STATE);
  };

  const clearMustChangePassword = () => {
    setState(prev => ({ ...prev, mustChangePassword: false }));
  };

  return (
    <AuthContext.Provider value={{
      ...state,
      login,
      register,
      logout,
      clearMustChangePassword,
      isAuthenticated: !!state.token,
      isAdmin: state.role === 'Admin',
    }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used inside AuthProvider');
  return ctx;
}
