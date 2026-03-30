import { createContext, useContext, useState, ReactNode } from 'react';
import userService from '@/services/userService';

interface AuthState {
  token: string | null;
  refreshToken: string | null;
  userId: number | null;
  username: string | null;
}

interface AuthContextType extends AuthState {
  login: (email: string, password: string) => Promise<void>;
  register: (email: string, password: string) => Promise<void>;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | null>(null);

function decodeJwtPayload(token: string): { sub?: string; unique_name?: string } {
  try {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')));
  } catch {
    return {};
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [state, setState] = useState<AuthState>(() => {
    const token = localStorage.getItem('token');
    const refreshToken = localStorage.getItem('refreshToken');
    if (token) {
      const payload = decodeJwtPayload(token);
      return {
        token,
        refreshToken,
        userId: payload.sub ? parseInt(payload.sub) : null,
        username: payload.unique_name ?? null,
      };
    }
    return { token: null, refreshToken: null, userId: null, username: null };
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
    };
    localStorage.setItem('token', dto.token);
    if (dto.refreshToken) localStorage.setItem('refreshToken', dto.refreshToken);
    setState(newState);
  };

  const register = async (email: string, password: string) => {
    const dto = await userService.register(email, password);
    if (!dto.isSuccess || !dto.token) throw new Error(dto.errorMessage ?? 'Registration failed');
    const payload = decodeJwtPayload(dto.token);
    const newState: AuthState = {
      token: dto.token,
      refreshToken: dto.refreshToken ?? null,
      userId: dto.userId ?? (payload.sub ? parseInt(payload.sub) : null),
      username: payload.unique_name ?? email,
    };
    localStorage.setItem('token', dto.token);
    if (dto.refreshToken) localStorage.setItem('refreshToken', dto.refreshToken);
    setState(newState);
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    setState({ token: null, refreshToken: null, userId: null, username: null });
  };

  return (
    <AuthContext.Provider value={{ ...state, login, register, logout, isAuthenticated: !!state.token }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used inside AuthProvider');
  return ctx;
}
