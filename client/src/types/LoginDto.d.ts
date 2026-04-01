type LoginDto = {
  isSuccess: boolean;
  errorMessage?: string;
  token?: string;
  refreshToken?: string;
  userId?: number;
  mustChangePassword?: boolean;
};
