export const API_BASE_URL = import.meta.env.VITE_API_URL;
export const HUB_BASE_URL = import.meta.env.VITE_API_BASE_URL as string;

export const API_KANA_URL = `${API_BASE_URL}/characters`;
export const API_LESSONS_URL = `${API_BASE_URL}/lessons`;
export const API_USERS_URL = `${API_BASE_URL}/users`;
export const API_USERS_SETTINGS_URL = `${API_BASE_URL}/users/settings`;
export const API_USERS_FORGOT_PASSWORD_URL = `${API_BASE_URL}/users/forgot-password`;
export const API_USERS_RESET_PASSWORD_URL = `${API_BASE_URL}/users/reset-password`;
export const API_ADMIN_URL = `${API_BASE_URL}/admin`;
export const API_GRAMMAR_URL = `${API_BASE_URL}/grammar`;
export const API_READING_URL = `${API_BASE_URL}/reading`;
export const API_PATH_URL = `${API_BASE_URL}/path`;
